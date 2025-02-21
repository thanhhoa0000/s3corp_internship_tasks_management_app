using System.Net;

namespace TaskManagementApp.Frontends.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProcessor _tokenProcessor;
        private readonly ILogger<BaseService> _logger;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenProcessor tokenHandler, ILogger<BaseService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProcessor = tokenHandler;
            _logger = logger;
        }

        public async Task<Response?> SendAsync(Request request, bool bearer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MyTasksApp");
                HttpRequestMessage message = new HttpRequestMessage();

                message.Headers.Add("Accept", "application/json");

                if (bearer)
                {
                    var token = _tokenProcessor.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }

                message.RequestUri = new Uri(request.Url);

                if (request.Body is not null)
                    message.Content = new StringContent(JsonSerializer.Serialize(request.Body), Encoding.UTF8, "application/json");

                _logger.LogDebug($"\nRequest: {request.Body}");

                HttpResponseMessage? responseMessage = null;

                switch (request.ApiMethod)
                {
                    case ApiMethod.Post:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiMethod.Put:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiMethod.Delete:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                responseMessage = await client.SendAsync(message);

                switch (responseMessage.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var content = await responseMessage.Content.ReadAsStringAsync();
                        _logger.LogDebug($"\nContent string: {content}");

                        var response = new Response();

                        if ((content.StartsWith("{") && content.EndsWith("}")) ||
                            (content.StartsWith("[") && content.EndsWith("]")))
                        {
                            response.Body = JsonSerializer.Deserialize<JsonDocument>(content);
                        }
                        else
                            response.Message = content.Trim();

                        return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error(s) occured:\n-----{ex}");

                var response = new Response
                {
                    Message = "Error(s) occured!",
                    IsSuccess = false
                };

                return response;
            }
        }
    }
}
