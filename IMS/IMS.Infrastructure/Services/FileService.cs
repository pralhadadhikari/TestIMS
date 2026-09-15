using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly string _rootPath;

        public FileService(IConfiguration configuration)
        {
            _rootPath = configuration["FileStorage:RootPath"]
                ?? throw new InvalidOperationException(
                    "FileStorage:RootPath is not configured.");
        }

        public async Task<string> SaveFileAsync(IFormFile file,string folder, CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid image.");
            // Allow large phone photos as input, but don't store them directly.
            const long maxUploadSize = 20 * 1024 * 1024;

            if (file.Length > maxUploadSize)
                throw new ArgumentException(
                    "Image size cannot exceed 20 MB.");

            var directory = Path.Combine(_rootPath, folder);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Keep small images as they are
            const long compressFromSize = 500 * 1024; // 500 KB

            if (file.Length <= compressFromSize)
            {
                var extension = Path.GetExtension(file.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(directory, fileName);

                await using var stream = new FileStream(
                    filePath,
                    FileMode.Create);

                await file.CopyToAsync(stream, cancellationToken);

                return $"{folder}/{fileName}";
            }

            // Compress large images
            var compressedFileName = $"{Guid.NewGuid()}.jpg";
            var compressedFilePath = Path.Combine(
                directory,
                compressedFileName);

            using var image = await Image.LoadAsync(
                file.OpenReadStream(),
                cancellationToken);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(1600, 1600)
            }));

            var encoder = new JpegEncoder
            {
                Quality = 80
            };

            await image.SaveAsJpegAsync(
                compressedFilePath,
                encoder,
                cancellationToken);

            return $"{folder}/{compressedFileName}";
        }

        

        public Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Task.CompletedTask;

            var fullPath = Path.Combine(
                _rootPath,
                filePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }

    }

}
