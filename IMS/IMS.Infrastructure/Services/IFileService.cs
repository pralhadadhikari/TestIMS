using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder, CancellationToken cancellationToken = default);
        Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default);
    }
}
