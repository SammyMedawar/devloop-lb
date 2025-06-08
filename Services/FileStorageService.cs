using DevLoopLB.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace DevLoopLB.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _wwwRootPath;
        private readonly ILogger<FileStorageService> _logger;
        private readonly string[] _convertToWebP = { ".jpg", ".jpeg", ".png" };

        public FileStorageService(IWebHostEnvironment webHostEnvironment, ILogger<FileStorageService> logger)
        {
            _wwwRootPath = webHostEnvironment.WebRootPath ?? throw new InvalidOperationException("WebRootPath is not configured");
            _logger = logger;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty");
            }

            var randomGuid = Guid.NewGuid().ToString();
            var originalExtension = Path.GetExtension(file.FileName);
            var shouldConvertToWebP = _convertToWebP.Contains(originalExtension);
            var finalExtension = shouldConvertToWebP ? ".webp" : originalExtension;

            var fileName = randomGuid + finalExtension;

            var folderPath = Path.Combine(_wwwRootPath, folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            try
            {
                if (shouldConvertToWebP)
                {
                    await ConvertToWebPAsync(file, filePath);
                    _logger.LogInformation("Converted {OriginalExtension} to WebP: {FileName}", originalExtension, fileName);
                }
                else
                {
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                    _logger.LogInformation("Saved file without conversion: {FileName}", fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving file {FileName}", file.FileName);
                throw new InvalidOperationException($"Failed to save file: {ex.Message}");
            }
        }

        private async Task ConvertToWebPAsync(IFormFile file, string outputPath)
        {
            using var inputStream = file.OpenReadStream();
            using var image = await Image.LoadAsync(inputStream);
            var webpEncoder = new WebpEncoder
            {
                Quality = 85,
                Method = WebpEncodingMethod.BestQuality
            };

            await image.SaveAsync(outputPath, webpEncoder);
        }

        public async Task<List<string>> SaveMultipleFilesAsync(List<IFormFile> files, string folderName)
        {
            var savedFilePaths = new List<string>();

            try
            {
                foreach (var file in files)
                {
                    var savedPath = await SaveFileAsync(file, folderName);
                    savedFilePaths.Add(savedPath);
                }
                return savedFilePaths;
            }
            catch
            {
                await DeleteMultipleFilesAsync(savedFilePaths);
                throw;
            }
        }

        public async Task DeleteFileAsync(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return;

            try
            {
                var fullPath = GetFullPath(relativePath);
                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                    _logger.LogInformation("Deleted file: {FilePath}", relativePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file {FilePath}", relativePath);
            }
        }

        public async Task DeleteMultipleFilesAsync(List<string> filePaths)
        {
            var deleteTasks = filePaths.Select(DeleteFileAsync);
            await Task.WhenAll(deleteTasks);
        }

        public bool FileExists(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return false;

            var fullPath = GetFullPath(relativePath);
            return File.Exists(fullPath);
        }

        public string GetFullPath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return string.Empty;

            return Path.Combine(_wwwRootPath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        }
    }
}