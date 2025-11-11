using HRsystem.Api.Shared.DTO;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace HRsystem.Api.Services.DocumentHandling
{
    public class DocumentService
    {
        private readonly DocumentSettings _settings;

        public DocumentService(IOptions<DocumentSettings> settings)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Upload a single file for a specific employee.
        /// </summary>
        public async Task<ResponseResultDTO<List<DocumentUploadResultDTO>>> UploadDocumentAsync(IFormFile file, string employeeCode, string docType)
        {
            if (file == null || file.Length == 0)
                return new ResponseResultDTO<List<DocumentUploadResultDTO>>
                {
                    Success = false,
                    Message = "No file selected for upload."
                };

            try
            {
                var result = await SaveFileAsync(file, employeeCode, docType);

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

        /// <summary>
        /// Upload multiple files for a specific employee.
        /// </summary>
        public async Task<ResponseResultDTO<List<DocumentUploadResultDTO>>> UploadDocumentsAsync(List<IFormFile> files, string employeeCode)
        {
            if (files == null || files.Count == 0)
                return new ResponseResultDTO<List<DocumentUploadResultDTO>>
                {
                    Success = false,
                    Message = "No files provided."
                };

            var results = new List<DocumentUploadResultDTO>();
            Stopwatch sw = Stopwatch.StartNew();

            foreach (var file in files)
            {
                try
                {
                    var docType = Path.GetExtension(file.FileName).Trim('.').ToUpperInvariant();
                    var result = await SaveFileAsync(file, employeeCode, docType);
                    results.Add(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error uploading file {file.FileName}: {ex.Message}");
                }
            }

            sw.Stop();
            Console.WriteLine($"Uploaded {results.Count} files in {sw.ElapsedMilliseconds} ms");

            return new ResponseResultDTO<List<DocumentUploadResultDTO>>
            {
                Success = true,
                Message = "All files processed.",
                Data = results
            };
        }

        // 🔹 Core logic reused by both single and multi upload
        private async Task<DocumentUploadResultDTO> SaveFileAsync(IFormFile file, string employeeCode, string docType)
        {
            string saveFolder = GetStaticFolder(employeeCode);
            string virtualFolder = GetVirtualFolder(employeeCode);

            var fileName = GenerateFileName(file);
            string staticFilePath = Path.Combine(saveFolder, fileName);
            string virtualFilePath = Path.Combine(virtualFolder, fileName).Replace("\\", "/");

            await using (var stream = new FileStream(staticFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string mimeType = GetMimeType(Path.GetExtension(file.FileName));
            long size = file.Length;

            return new DocumentUploadResultDTO
            {
                DocType = docType,
                MimeType = mimeType,
                FileName = fileName,
                DocFullPath = virtualFilePath,
                FileSizeBytes = size,
                FileSizeReadable = FormatFileSize(size),
                UploadedAt = DateTime.Now
            };
        }

        // 🔹 File name generator
        private static string GenerateFileName(IFormFile file)
        {
            string ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(ext) || ext == ".blob")
                ext = ".jpg";

            return $"{Guid.NewGuid()}{ext}";
        }

        // 🔹 Folder creation (physical)
        private string GetStaticFolder(string employeeCode)
        {
            string folderDate = $"{DateTime.Now:yyyy-MM-dd}";
            string basePath = Path.Combine(_settings.StaticFilesFolder, employeeCode, folderDate);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            return basePath;
        }

        // 🔹 Folder (virtual)
        private string GetVirtualFolder(string employeeCode)
        {
            string folderDate = $"{DateTime.Now:yyyy-MM-dd}";
            string basePath = Path.Combine(_settings.VirtualURL, employeeCode, folderDate);
            return basePath.Replace("\\", "/");
        }

        // 🔹 MIME type resolver
        public static string GetMimeType(string fileExtension)
        {
            if (string.IsNullOrWhiteSpace(fileExtension))
                return "application/octet-stream";

            fileExtension = fileExtension.Trim().ToLower();

            return fileExtension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".csv" => "text/csv",
                ".zip" => "application/zip",
                ".rar" => "application/x-rar-compressed",
                _ => "application/octet-stream"
            };
        }

        // 🔹 Human-readable file size (e.g. "125 KB")
        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }

    public class DocumentSettings
    {
        public string StaticFilesFolder { get; set; } = string.Empty;
        public string VirtualURL { get; set; } = string.Empty;
        public string DocumentPublicBaseAddress { get; set; } = string.Empty;
    }
}
