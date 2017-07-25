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
using MySql.Data;

namespace NutritionDoctorApi.Controllers
{
    [Route("api/user/[controller]")]
    public class IdentifyController : Controller
    {
        // GET api/user/identify/5
        [HttpGet("{userId}")]
        public string Get(string data)
        {
            var ConnString = "database=pingandb;data source=us-cdbr-azure-east-c.cloudapp.net;user id=b8639718fe5ad6;password=2cd7b667";
            var MySqlConnection = new MySql.Data.MySqlClient.MySqlConnection(ConnString);

            try
            {
                MySqlConnection.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return "error connecting to database:" + ex;
            }

            var UserSqlQuery = "SELECT user.DETECTED_FOOD, user.PHOTO_URL FROM user_food_tbl as user WHERE user_id = \"_userId\" AND DETECTED_FOOD_SOURCE = \"AzureMachineLearning\"";
            var FoodFactsSqlQuery = "select * from food_facts_tbl as facts where facts.FOOD_NAME = \"_foodName\"";
            MySql.Data.MySqlClient.MySqlCommand Command = new MySql.Data.MySqlClient.MySqlCommand(UserSqlQuery, MySqlConnection);
            Command.Parameters.AddWithValue("_userId", data);
            MySql.Data.MySqlClient.MySqlDataReader SqlReader = Command.ExecuteReader();
            List<UserFoodData> results = new List<UserFoodData>();
            var TestFoodFacts = new FoodFacts();
            TestFoodFacts = new FoodFacts();
            TestFoodFacts.fat = "2.80g";
            TestFoodFacts.protein = "6.76g";
            TestFoodFacts.carbohydrate = "8.29g";
            TestFoodFacts.calories = "85kcal";
            TestFoodFacts.fiber = "1.0g";
            TestFoodFacts.sugar = "1.74g";
            while (SqlReader.Read())
            {
                var userFoodInfo = new UserFoodData();
                userFoodInfo.foodName = (string) SqlReader[1];
                userFoodInfo.userId = (string)data;
                userFoodInfo.imageUrl = (string)SqlReader[2];
                results.Add(userFoodInfo);
            }
            SqlReader.Close();

            foreach (var item in results)
            {
                Command.CommandText = FoodFactsSqlQuery;
                //MySql.Data.MySqlClient.MySqlCommand FoodFactCommand = new MySql.Data.MySqlClient.MySqlCommand(FoodFactsSqlQuery, MySqlConnection);
                Command.Parameters.AddWithValue("_foodName", item.foodName);
                MySql.Data.MySqlClient.MySqlDataReader FoodFactReader = Command.ExecuteReader();
                while (FoodFactReader.Read())
                {

                }
                FoodFactReader.Close();

            }
            MySqlConnection.Close();

            return JsonConvert.SerializeObject(results);
        }

        // POST api/user/identify
        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync([FromBody] IdentifyRequest request)
        {
            BlobService blobService = new BlobService();
            string imageBlobUri = await blobService.UploadImageToBlob(request.userId, request.imageData);
            await blobService.AddJobToQueue(request.userId, imageBlobUri);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}
