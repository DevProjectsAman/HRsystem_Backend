using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Xml.Linq;

namespace HRsystem.Api.Features.Documents
{
    public record UploadEmployeeDocumentCommand(
    IFormFile File 
) : IRequest<ResponseResultDTO<List<DocumentUploadResultDTO>>>;


    public class UploadEmployeeDocumentHandler
    : IRequestHandler<UploadEmployeeDocumentCommand, ResponseResultDTO<List<DocumentUploadResultDTO>>>
    {
       // private readonly DocumentSettings _settings;
        private readonly IConfiguration _configuration;

        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public UploadEmployeeDocumentHandler(IConfiguration config, DBContextHRsystem db,ICurrentUserService currentUserService)
        {
            _configuration = config;
            _db = db;
            _currentUser = currentUserService;
        }

        public async Task<ResponseResultDTO<List<DocumentUploadResultDTO>>> Handle(
            UploadEmployeeDocumentCommand request,
            CancellationToken ct)
        {
            var file = request.File;

            if (file == null || file.Length == 0)
            {
                return new ResponseResultDTO<List<DocumentUploadResultDTO>>
                {
                    Success = false,
                    Message = "No file selected for upload."
                    ,
                    Data = null
                };
            }

            try
            {
                var result = await SaveFileAsync(file, ct);

                return new ResponseResultDTO<List<DocumentUploadResultDTO>>
                {
                    Success = true,
                    Message = "File uploaded successfully.",
                    Data = new List<DocumentUploadResultDTO> { result }
                };
            }
            catch (Exception ex)
            {
                return new ResponseResultDTO<List<DocumentUploadResultDTO>>
                {
                    Success = false,
                    Message = $"Error uploading file: {ex.Message}"
                };
            }
        }

        // ========================= INTERNAL LOGIC =========================


        public record DocumentInfo(string MimeType, string Type);


        private async Task<DocumentUploadResultDTO> SaveFileAsync(
            IFormFile file,
             
            CancellationToken ct)
        {

            string employeeId = GenerateUniqueEmployeeCode();

            string saveFolder = GetStaticFolder(employeeId);


            string virtualFolder = GetVirtualFolder(employeeId);

            var fileName = GenerateFileName(file);
            var staticPath = Path.Combine(saveFolder, fileName);
            var virtualPath = Path.Combine(virtualFolder, fileName).Replace("\\", "/");

            await using var stream = new FileStream(staticPath, FileMode.Create);
            await file.CopyToAsync(stream, ct);

            var size = file.Length;

            var EmpCodeTrack = new TbEmployeeCodeTracking()
            {
                UniqueEmployeeCode = employeeId,
                IsUsed = false,
                GeneratedAt = DateTime.UtcNow,
                GeneratedById = _currentUser.UserId,
                FolderPath = saveFolder ,
                 DocFullPath = Path.Combine( saveFolder, fileName)
                 


            };

            var empCodeEntry = _db.TbEmployeeCodeTrackings.Add(EmpCodeTrack);
            await  _db.SaveChangesAsync(ct);

            var docInfo = GetDocumentInfo(Path.GetExtension(file.FileName));

            return new DocumentUploadResultDTO
            {
                DocType = docInfo.Type,
                MimeType = docInfo.MimeType,
                FileName = fileName,
                DocFullPath = virtualPath,
                FileSizeBytes = size,
                FileSizeReadable = FormatFileSize(size),
                UploadedAt = DateTime.UtcNow,
                 UniqueEmployeeCode = employeeId
                  
            };
        }

        private static string GenerateFileName(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(ext) || ext == ".blob")
                ext = ".jpg";

            return $"{Guid.NewGuid()}{ext}";
        }

        private string GetStaticFolder(string employeeCode)
        {
            var imageSettings = _configuration.GetSection("ImageSettings");
            var StaticFilesFolder = imageSettings["StaticFilesFolder"];
             


            var folderDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var path = Path.Combine(StaticFilesFolder, folderDate, employeeCode );

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        private string GetVirtualFolder(string employeeCode)
        {
            var folderDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var imageSettings = _configuration.GetSection("ImageSettings");
            var VirtualURL = imageSettings["VirtualURL"];


            return Path.Combine(VirtualURL, folderDate, employeeCode )
                .Replace("\\", "/");
        }

        //private static string GetMimeType(string ext) => ext?.ToLower() switch
        //{
        //    ".jpg" or ".jpeg" => "image/jpeg",
        //    ".png" => "image/png",
        //    ".pdf" => "application/pdf",
        //    ".doc" => "application/msword",
        //    ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        //    ".xls" => "application/vnd.ms-excel",
        //    ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //    _ => "application/octet-stream"
        //};

        private static DocumentInfo GetDocumentInfo(string? ext) =>
    ext?.ToLowerInvariant() switch
    {
        ".jpg" or ".jpeg" => new("image/jpeg", "Image"),
        ".png" => new("image/png", "Image"),

        ".pdf" => new("application/pdf", "Pdf"),

        ".doc" => new("application/msword", "Word"),
        ".docx" => new("application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Word"),

        ".xls" => new("application/vnd.ms-excel", "Excel"),
        ".xlsx" => new("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Excel"),

        _ => new("application/octet-stream", "Unknown")
    };



        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        private static string GenerateUniqueEmployeeCode()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var shortGuid = Guid.NewGuid().ToString("N")[..8].ToUpper();
            string uq = $"EMP_{timestamp}_{shortGuid}";
            return uq;

        }


    }

    public static class DocumentEndpoints
        {
            public static void MapDocumentEndpoints(this IEndpointRouteBuilder app)
            {
                app.MapPost("/api/documents/uploadDoc", [Authorize]
                async (
                        HttpRequest request,
                        IMediator mediator) =>
                    {
                        var form = await request.ReadFormAsync();
                        var file = form.Files.GetFile("file");

                      

                        if (file == null)
                            return Results.BadRequest("File is required");

                        var result = await mediator.Send(
                            new UploadEmployeeDocumentCommand(file));

                        return Results.Ok(result);
                    })
                    .RequireAuthorization()
                    .WithName("UploadEmployeeDocument")
                    .Accepts<IFormFile>("multipart/form-data")
                    .Produces<ResponseResultDTO<List<DocumentUploadResultDTO>>>();
            }
        }


}
