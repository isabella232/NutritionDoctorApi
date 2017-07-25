using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace NutritionDoctorApi.Services
{
    public class BlobService 
    {
        /*
         * 
         *                "pinganhackfest2017",
               "Hi7yuNxb67pBoSqwhHlnXRHnDcLyZmuVpbmc38vzA0j5HclHVIei66jIz+p7Qa9wobC8kUzBDFyI8LCe/842Ug=="), true);
               */

        CloudBlobClient blobClient;
        CloudStorageAccount storageAccount;

        public BlobService()
        {
            storageAccount = new CloudStorageAccount(
               new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
               "pinganmlfunctio8acb",
               "6UFsmsghPNzORtfLqcmFiDbj98pbU1Jjbus/m2V15OS6VrfG+MxLK9yxafpuFqoztutBvJrHGEphF8tsYHdN2A=="), true);
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
            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference("queue-identifyjob");

            // Create the queue if it doesn't already exist.
            await queue.CreateIfNotExistsAsync();

            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage("{\"userId\" : \"" + userId + "\", \"imageUrl\" : \"" + imageUrl + "\"");
            await queue.AddMessageAsync(message);
        }
    }
}
