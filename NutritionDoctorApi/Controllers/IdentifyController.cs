using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NutritionDoctorApi.Model;
using NutritionDoctorApi.Services;

namespace NutritionDoctorApi.Controllers
{
    [Route("api/user/[controller]")]
    public class IdentifyController : Controller
    {
        Lazy<MySqlStore> _database = new Lazy<MySqlStore>(() => new MySqlStore());

        private MySqlStore Database { get { return _database.Value; } }

        // GET api/user/identify/5
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAsync(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var foodList = await Database.GetFoodByUserAsync(userId);
            foreach (var foodItem in foodList)
            {
                foodItem.nutrition = await Database.GetFoodFactsAsync(foodItem.foodName);
            }

            return Ok(foodList);
        }

        // POST api/user/identify
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] IdentifyRequest request)
        {
            BlobService blobService = new BlobService();
            string imageBlobUri = await blobService.UploadImageToBlob(request.userId, request.imageData);
            await blobService.AddJobToQueue(request.userId, imageBlobUri);
            return Ok();
        }
    }
}
