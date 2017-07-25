using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace NutritionDoctorApi.Services
{
    public class BlobService 
    {

        CloudBlobClient blobClient; 

        public BlobService()
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(
               new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
               "pinganhackfest2017",
               "Hi7yuNxb67pBoSqwhHlnXRHnDcLyZmuVpbmc38vzA0j5HclHVIei66jIz+p7Qa9wobC8kUzBDFyI8LCe/842Ug=="), true);

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
    }
}
