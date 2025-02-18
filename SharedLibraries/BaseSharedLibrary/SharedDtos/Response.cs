namespace TaskManagementApp.SharedLibraries.BaseSharedLibraries.SharedDtos
{
    public class Response
    {
        public object? Body { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
