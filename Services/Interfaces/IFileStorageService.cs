namespace DevLoopLB.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        Task DeleteFileAsync(string filePath);
        bool FileExists(string filePath);
        string GetFullPath(string relativePath);
        Task<List<string>> SaveMultipleFilesAsync(List<IFormFile> files, string folderName);
        Task DeleteMultipleFilesAsync(List<string> filePaths);
    }
}
