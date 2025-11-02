namespace HRsystem.Api.Shared.DTO
{

    public class DocumentUploadResultDTO
    {
        public string DocType { get; set; } = string.Empty;      // e.g. "PDF"
        public string MimeType { get; set; } = string.Empty;     // e.g. "application/pdf"
        public string FileName { get; set; } = string.Empty;     // e.g. "7d0b...pdf"
        public string DocFullPath { get; set; } = string.Empty;  // virtual URL path
        public long FileSizeBytes { get; set; }                  // e.g. 124556
        public string FileSizeReadable { get; set; } = string.Empty; // e.g. "121 KB"
        public DateTime UploadedAt { get; set; }                 // UTC timestamp
    }

}
