using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NutritionDoctorApi.Model;
using NutritionDoctorApi.Services;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace NutritionDoctorApi.Controllers
{
    [Route("api/user/[controller]")]
    public class IdentifyController : Controller
    {
        // GET api/user/identify/5
        [HttpGet("{userId}")]
        public IEnumerable<string> Get(string data)

        {
            UserFoodData info = new UserFoodData();
            info.foodName = "Chicken Chow Mein";
            info.userId = "lilian";
            info.imageUrl = "https://pinganhackfest2017.blob.core.windows.net/chow-mein/thumb_275%20(1).jpeg";
            info.nutrition = new FoodFacts();
            info.nutrition.fat = "2.80g";
            info.nutrition.protein = "6.76g";
            info.nutrition.carbohydrate = "8.29g";
            info.nutrition.calories = "85kcal";
            info.nutrition.fiber = "1.0g";
            info.nutrition.sugar = "1.74g";

            UserFoodData otherInfo = new UserFoodData();
            info.foodName = "Kung Pao Chicken";
            info.userId = "lilian";
            info.imageUrl = "https://pinganhackfest2017.blob.core.windows.net/kung-pao/thumb_275%20(1).jpeg";
            info.nutrition = new FoodFacts();
            info.nutrition.fat = "2.80g";
            info.nutrition.protein = "6.76g";
            info.nutrition.carbohydrate = "8.29g";
            info.nutrition.calories = "85kcal";
            info.nutrition.fiber = "1.0g";
            info.nutrition.sugar = "1.74g";

            UserFoodData[] results= { info, otherInfo };
            yield return JsonConvert.SerializeObject(results);
        }

        // POST api/user/identify
        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync([FromBody] IdentifyRequest request)
        {
            BlobService blobService = new BlobService();
            string imageBlobUri = await blobService.UploadImageToBlob(request.userId, request.imageData);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}
