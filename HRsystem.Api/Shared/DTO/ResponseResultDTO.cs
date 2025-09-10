namespace HRsystem.Api.Shared.DTO
{
     

    public class ResponseErrorDTO
    {
        public string Property { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
    public class ResponseResultDTO<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        // New field: Validation / error details
        public List<ResponseErrorDTO>? Errors { get; set; }

        public ResponseResultDTO() { }

        public ResponseResultDTO(string message) => Message = message;

        public ResponseResultDTO(string message, T obj)
        {
            Message = message;
            Data = obj;
        }
    }

    public class ResponseResultDTO
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        // Non-generic variant can also have errors
        public List<ResponseErrorDTO>? Errors { get; set; }
    }
}
