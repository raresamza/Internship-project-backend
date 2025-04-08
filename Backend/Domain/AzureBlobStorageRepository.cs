using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Backend.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Backend.Infrastructure
{
    public class AzureBlobStorageRepository : IFileStorageRepository
    {
        private readonly string _containerName;
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobStorageRepository(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            _containerName = configuration["AzureStorage:ContainerName"];
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(fileName);

            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString(); // This is your FileUrl stored in DB
        }

        public async Task<Stream?> DownloadFileAsync(string fileUrl)
        {
            var uri = new Uri(fileUrl);

            // Parse blob path from the URL
            string containerName = uri.Segments[1].TrimEnd('/');
            string blobName = string.Join("", uri.Segments.Skip(2));

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            if (await blobClient.ExistsAsync())
            {
                var download = await blobClient.DownloadAsync();
                return download.Value.Content;
            }

            return null;
        }
    }
}
