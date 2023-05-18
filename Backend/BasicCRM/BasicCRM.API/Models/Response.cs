namespace BasicCRM.API.Models
{
    public class Response
    {
        public object? Content { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public IDictionary<string, string[]> Errors { get; set; }
    }
}
