namespace TaskManagementApp.Frontends.Web.Services
{
    public class TokenProcessor : ITokenProcessor
    {
        private readonly IHttpContextAccessor _accessor;

        public TokenProcessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public void ClearToken()
            => _accessor.HttpContext?.Response.Cookies.Delete(CookieProperties.JwtCookie);
        

        public string? GetToken()
        {
            string? token = null;
            bool? hasToken = _accessor
                .HttpContext?.Request.Cookies.TryGetValue(CookieProperties.JwtCookie, out token);

            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
            => _accessor.HttpContext?.Response.Cookies.Append(
                CookieProperties.JwtCookie, token);        
    }
}
