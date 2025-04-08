using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Backend.Application.Abstractions;

public interface IFileStorageRepository
{
    Task<string> UploadFileAsync(IFormFile file, string fileName);
    Task<Stream?> DownloadFileAsync(string fileName);
}
