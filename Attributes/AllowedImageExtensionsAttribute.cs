using System.ComponentModel.DataAnnotations;

namespace DevLoopLB.Attributes
{
    public class AllowedImageExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        private readonly long _maxFileSize;
        public AllowedImageExtensionsAttribute(long maxFileSizeInBytes, params string[] extensions)
        {
            _extensions = extensions;
            _maxFileSize = maxFileSizeInBytes;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();

                if (string.IsNullOrEmpty(extension))
                {
                    return new ValidationResult("File must have an extension.");
                }

                if (!_extensions.Contains(extension))
                {
                    var allowedExtensions = string.Join(", ", _extensions);
                    return new ValidationResult($"Only {_extensions} files are allowed.");
                }

                if (file.Length > _maxFileSize)
                {
                    var maxSizeMB = _maxFileSize / (1024 * 1024);
                    return new ValidationResult($"File size must not exceed {maxSizeMB}MB.");
                }

                if (file.Length == 0)
                {
                    return new ValidationResult("File cannot be empty.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
