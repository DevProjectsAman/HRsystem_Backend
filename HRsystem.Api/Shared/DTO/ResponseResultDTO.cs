namespace HRsystem.Api.Shared.DTO
{
    public class ResponseErrorDTO
    {
        public string Property { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }

    public class ResponseResultDTO<T>
    {
        private T? _data;

        public T? Data
        {
            get => _data;
            set
            {
                _data = value;
                // Automatically set TotalCount if Data is a collection
                if (value is System.Collections.ICollection collection)
                {
                    TotalCount = collection.Count;
                }
                else if (value == null)
                {
                    TotalCount = 0;
                }
                else
                {
                    TotalCount = 1; // for single objects
                }
            }
        }

        public int TotalCount { get; private set; }

        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        // Validation / error details
        public List<ResponseErrorDTO>? Errors { get; set; }

        public ResponseResultDTO() { }

        public ResponseResultDTO(string message) => Message = message;

        public ResponseResultDTO(string message, T obj)
        {
            Message = message;
            Data = obj; // Will auto-set TotalCount
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
