using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace HRsystem.Api.Features.Documents;

// ========================= COMMAND =========================

public sealed record UploadEmployeeDocumentCommand(IFormFile File)
    : IRequest<ResponseResultDTO<List<DocumentUploadResultDTO>>>;

// ========================= HANDLER =========================

public sealed class UploadEmployeeDocumentHandler
    : IRequestHandler<UploadEmployeeDocumentCommand, ResponseResultDTO<List<DocumentUploadResultDTO>>>
{
    private readonly DBContextHRsystem _db;
    private readonly ICurrentUserService _currentUser;
    private readonly IConfiguration _configuration;

    public UploadEmployeeDocumentHandler(
        DBContextHRsystem db,
        ICurrentUserService currentUser,
        IConfiguration configuration)
    {
        _db = db;
        _currentUser = currentUser;
        _configuration = configuration;
    }

    public async Task<ResponseResultDTO<List<DocumentUploadResultDTO>>> Handle(
        UploadEmployeeDocumentCommand request,
        CancellationToken ct)
    {
        if (request.File is not { Length: > 0 })
        {
            return new ResponseResultDTO<List<DocumentUploadResultDTO>>
            {
                Success = false,
                Message = "No file selected for upload.",
                Data = null
            };
        }

        try
        {
            var result = await SaveFileAsync(request.File, ct);
            return new ResponseResultDTO<List<DocumentUploadResultDTO>>
            {
                Success = true,
                Message = "File uploaded successfully.",
                Data = [result]
            };
        }
        catch (Exception ex)
        {
            return new ResponseResultDTO<List<DocumentUploadResultDTO>>
            {
                Success = false,
                Message = $"Error uploading file: {ex.Message}",
                Data = null
            };
        }
    }

    private async Task<DocumentUploadResultDTO> SaveFileAsync(IFormFile file, CancellationToken ct)
    {
        var employeeCode = GenerateUniqueEmployeeCode();
        var fileName = GenerateFileName(file);
        var (staticPath, virtualPath) = GetFilePaths(employeeCode, fileName);

        await SaveFileToDiskAsync(file, staticPath, ct);
        await SaveEmployeeCodeTrackingAsync(employeeCode, staticPath, ct);

        var docInfo = GetDocumentInfo(Path.GetExtension(file.FileName));

        return new DocumentUploadResultDTO
        {
            DocType = docInfo.Type,
            MimeType = docInfo.MimeType,
            FileName = fileName,
            DocFullPath = virtualPath,
            FileSizeBytes = file.Length,
            FileSizeReadable = FormatFileSize(file.Length),
            UploadedAt = DateTime.UtcNow,
            UniqueEmployeeCode = employeeCode
        };
    }

    private static async Task SaveFileToDiskAsync(IFormFile file, string path, CancellationToken ct)
    {
        await using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream, ct);
    }

    private async Task SaveEmployeeCodeTrackingAsync(
        string employeeCode,
        string docFullPath,
        CancellationToken ct)
    {
        var tracking = new TbEmployeeCodeTracking
        {
            UniqueEmployeeCode = employeeCode,
            IsUsed = false,
            GeneratedAt = DateTime.UtcNow,
            GeneratedById = _currentUser.UserId,
            FolderPath = Path.GetDirectoryName(docFullPath)!,
            DocFullPath = docFullPath
        };

        _db.TbEmployeeCodeTrackings.Add(tracking);
        await _db.SaveChangesAsync(ct);
    }

    private (string StaticPath, string VirtualPath) GetFilePaths(string employeeCode, string fileName)
    {
        var staticFolder = GetStaticFolder(employeeCode);
        var virtualFolder = GetVirtualFolder(employeeCode);

        var staticPath = Path.Combine(staticFolder, fileName);
        var virtualPath = Path.Combine(virtualFolder, fileName).Replace("\\", "/");

        return (staticPath, virtualPath);
    }

    private string GetStaticFolder(string employeeCode)
    {
        var settings = _configuration.GetSection("ImageSettings");
        var baseFolder = settings["StaticFilesFolder"]
            ?? throw new InvalidOperationException("StaticFilesFolder not configured");

        var folderDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var path = Path.Combine(baseFolder, folderDate, employeeCode);

        Directory.CreateDirectory(path);
        return path;
    }

    private string GetVirtualFolder(string employeeCode)
    {
        var settings = _configuration.GetSection("ImageSettings");
        var virtualUrl = settings["VirtualURL"]
            ?? throw new InvalidOperationException("VirtualURL not configured");

        var folderDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
        return Path.Combine(virtualUrl, folderDate, employeeCode).Replace("\\", "/");
    }

    private static string GenerateFileName(IFormFile file)
    {
        var ext = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(ext) || ext == ".blob")
            ext = ".jpg";

        return $"{Guid.NewGuid():N}{ext}";
    }

    private static string GenerateUniqueEmployeeCode()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var shortGuid = Guid.NewGuid().ToString("N")[..8].ToUpper();
        return $"EMP_{timestamp}_{shortGuid}";
    }

    private static DocumentInfo GetDocumentInfo(string? ext) => ext?.ToLowerInvariant() switch
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
        string[] sizes = ["B", "KB", "MB", "GB", "TB"];
        double len = bytes;
        int order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }

    private sealed record DocumentInfo(string MimeType, string Type);
}

// ========================= ENDPOINTS =========================

public static class DocumentEndpoints
{
    public static void MapDocumentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/documents").WithTags("Documents");

        // --- Upload Document (POST) ---
        group.MapPost("/uploadDoc", [Authorize] async (IFormFile file, IMediator mediator) =>
        {
            if (file is null)
            {
                return Results.Ok(new ResponseResultDTO<List<DocumentUploadResultDTO>>
                {
                    Success = false,
                    Message = "File is required",
                    Data = null
                });
            }

            var result = await mediator.Send(new UploadEmployeeDocumentCommand(file));
            return result.Success
                ? Results.Ok(result)
                : Results.BadRequest(result);
        })
            .DisableAntiforgery()
            .RequireAuthorization()
            .WithName("UploadEmployeeDocument")
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<ResponseResultDTO<List<DocumentUploadResultDTO>>>()
            .Produces(StatusCodes.Status400BadRequest);

        // --- Get Document (GET) ---
        group.MapGet("/get", [Authorize] (string virtualPath, IConfiguration config) =>
        {
            if (string.IsNullOrWhiteSpace(virtualPath))
            {
                return Results.BadRequest(new
                {
                    Success = false,
                    Message = "virtualPath is required"
                });
            }

         //   var physicalPath = ResolvePhysicalPath(virtualPath, config);
            var physicalPath = virtualPath;


            if (physicalPath is null || !File.Exists(physicalPath))
            {
                return Results.NotFound(new
                {
                    Success = false,
                    Message = "File not found"
                });
            }

            var contentType = GetContentType(Path.GetExtension(physicalPath));
            var stream = File.OpenRead(physicalPath);

            return Results.File(stream, contentType);
        })
            .RequireAuthorization()
            .WithName("GetDocumentFile")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
    }
    private static string? ResolvePhysicalPath(string virtualPath, IConfiguration config)
    {
        var settings = config.GetSection("ImageSettings");
        var staticFolder = settings["StaticFilesFolder"];
        //var virtualUrl = settings["VirtualURL"]?.Replace("\\", "/").TrimEnd('/');
        var virtualUrl = string.Empty;


        if (string.IsNullOrEmpty(staticFolder))
            return null;

        var normalized = virtualPath.Replace("\\", "/").TrimStart('/');

        if (!string.IsNullOrEmpty(virtualUrl))
        {
            var prefix = virtualUrl.TrimStart('/');
            if (normalized.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                normalized = normalized[prefix.Length..].TrimStart('/');
        }

        return Path.Combine(staticFolder, normalized.Replace("/", Path.DirectorySeparatorChar.ToString()));
    }

    private static string GetContentType(string ext) => ext.ToLowerInvariant() switch
    {
        ".jpg" or ".jpeg" => "image/jpeg",
        ".png" => "image/png",
        ".pdf" => "application/pdf",
        ".doc" => "application/msword",
        ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        ".xls" => "application/vnd.ms-excel",
        ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        _ => "application/octet-stream"
    };
}

