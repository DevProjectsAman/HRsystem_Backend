namespace HRsystem.Api.Shared.DTO
{
    public class DocumentSettings
    {
        public string StaticFilesFolder { get; set; } = string.Empty;
        public string VirtualURL { get; set; } = string.Empty;
        public string DocumentPublicBaseAddress { get; set; } = string.Empty;
    }
}
