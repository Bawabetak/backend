namespace bawabetak_backend.Services.Interface
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, FileCategory category);

        string GetFullUrl(string fileName, FileCategory category);

        void DeleteFile(string fileName, FileCategory category);
        Task<List<string>> SaveFilesAsync(IEnumerable<IFormFile> files, FileCategory category);
        void DeleteFiles(IEnumerable<string> fileNames, FileCategory category);
        Task<string> ReplaceFileAsync(string oldFileName, IFormFile newFile, FileCategory category);
        Task<List<string>> ReplaceFilesAsync(IEnumerable<string> oldFileNames, IEnumerable<IFormFile> newFiles, FileCategory category);
    }
}
