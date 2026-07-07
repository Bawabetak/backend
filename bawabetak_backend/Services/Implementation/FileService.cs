

namespace bawabetak_backend.Services.Implementation
{
    public class FileService : IFileService
    {
        private readonly IHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string UploadsFolder = "uploads";

        public FileService(IHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetFolderName(FileCategory category) => category switch
        {
            FileCategory.UserProfile => "users/profiles",
            FileCategory.UserVerification => "users/verifications",
            FileCategory.ProjectAssets => "projects/assets",
            _ => "misc"
        };

        public async Task<string> SaveFileAsync(IFormFile file, FileCategory category)
        {
            FileValidator.Validate(file);

            string folderName = GetFolderName(category);
            string targetDirectory = Path.Combine(_environment.ContentRootPath, "wwwroot", UploadsFolder, folderName);

            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            string uniqueFileName = $"{Guid.NewGuid()}{extension}";
            string filePath = Path.Combine(targetDirectory, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return uniqueFileName;
        }

        public void DeleteFile(string fileName, FileCategory category)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            string folderName = GetFolderName(category);
            string filePath = Path.Combine(_environment.ContentRootPath, "wwwroot", UploadsFolder, folderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public async Task<List<string>> SaveFilesAsync(IEnumerable<IFormFile> files, FileCategory category)
        {
            if (files == null || !files.Any()) return new List<string>();

            var savedFileNames = new List<string>();
            foreach (var file in files)
            {
                string fileName = await SaveFileAsync(file, category);
                savedFileNames.Add(fileName);
            }

            return savedFileNames;
        }

        public void DeleteFiles(IEnumerable<string> fileNames, FileCategory category)
        {
            if (fileNames == null || !fileNames.Any()) return;

            foreach (var fileName in fileNames)
            {
                DeleteFile(fileName, category);
            }
        }

        public async Task<string> ReplaceFileAsync(string oldFileName, IFormFile newFile, FileCategory category)
        {
            if (newFile == null || newFile.Length == 0) return oldFileName;

            string newFileName = await SaveFileAsync(newFile, category);

            DeleteFile(oldFileName, category);

            return newFileName;
        }

        public async Task<List<string>> ReplaceFilesAsync(IEnumerable<string> oldFileNames, IEnumerable<IFormFile> newFiles, FileCategory category)
        {
          

            var newfiles= await SaveFilesAsync(newFiles, category);
            if (oldFileNames != null && oldFileNames.Any())
            {
                DeleteFiles(oldFileNames, category);
            }
            return newfiles;
        }

        public string GetFullUrl(string fileName, FileCategory category)
        {
            if (string.IsNullOrEmpty(fileName)) return string.Empty;

            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return string.Empty;

            string baseUrl = $"{request.Scheme}://{request.Host}";
            string folderName = GetFolderName(category);

            return $"{baseUrl}/{UploadsFolder}/{folderName}/{fileName}";
        }
    }
}