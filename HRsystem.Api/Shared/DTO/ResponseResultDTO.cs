namespace HRsystem.Api.Shared.DTO
{
    public class ResponseResultDTO<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        public ResponseResultDTO() { }
        public ResponseResultDTO(string message) => Message = message;
        public ResponseResultDTO(string message, T obj) { Message = message; Data = obj; }


    }
    public class ResponseResultDTO
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
