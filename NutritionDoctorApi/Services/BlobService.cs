using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.Extensions.Configuration;

namespace NutritionDoctorApi.Services
{
    public class BlobService 
    {
        CloudBlobClient blobClient;
        CloudStorageAccount storageAccount;
        public static IConfigurationRoot Configuration { get; set; }
        public BlobService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            storageAccount = new CloudStorageAccount(
               new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
               $"{Configuration["storage_id"]}",
               $"{Configuration["storage_key"]}"), true);

            blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task<string> UploadImageToBlob(string userId, string imageData)
        {
            CloudBlobContainer container = blobClient.GetContainerReference(userId);
            await container.CreateIfNotExistsAsync();

            // Set public access to blob so we can easily use the URL
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            Guid guid = Guid.NewGuid();
            CloudBlockBlob foodImageBlob = container.GetBlockBlobReference(guid.ToString());
            byte[] imageBytes = Convert.FromBase64String(imageData);
            await foodImageBlob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);
            var blobUri = foodImageBlob.StorageUri.PrimaryUri.AbsoluteUri;
            return blobUri;
        }

        public async Task AddJobToQueue(string userId, string imageUrl)
        {
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("queue-identifyjob");
            await queue.CreateIfNotExistsAsync();
            CloudQueueMessage message = new CloudQueueMessage("{\"UserId\" : \"" + userId + "\", \"ImageUrl\" : \"" + imageUrl + "\"}");
            await queue.AddMessageAsync(message);
        }
    }
}
