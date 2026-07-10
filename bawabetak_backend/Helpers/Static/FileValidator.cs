
namespace bawabetak_backend.Helpers.Static
{
    public static class FileValidator
    {
        private static readonly long MaxFileSize = 20 * 1024 * 1024;

        private static readonly string[] AllowedExtensions =
        {
            ".jpg", ".jpeg", ".png", ".gif", ".webp",
            ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt",
            ".mp3", ".wav", ".m4a", ".ogg", ".aac"
        };

        private static readonly string[] AllowedContentTypes =
        {
            "image/jpeg", "image/png", "image/gif", "image/webp",
            "application/pdf", "text/plain",
            "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "application/vnd.ms-powerpoint", "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            "audio/mpeg", "audio/wav", "audio/x-m4a", "audio/ogg", "audio/aac", "audio/mp3"
        };

        public static void Validate(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestCustomException(ResponseKeys.FileIsRequired);

            if (file.Length > MaxFileSize)
                throw new BadRequestCustomException(ResponseKeys.FileSizeExceeded);

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                throw new BadRequestCustomException(ResponseKeys.InvalidFileType);

            var contentType = file.ContentType.ToLowerInvariant();
            if (!AllowedContentTypes.Contains(contentType))
                throw new BadRequestCustomException(ResponseKeys.InvalidFileContentType);
        }
    }
}
