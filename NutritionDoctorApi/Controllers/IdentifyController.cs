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
        Lazy<MySqlStore> _database = new Lazy<MySqlStore>(() => new MySqlStore());

        private MySqlStore Database { get { return _database.Value; } }

        // GET api/user/identify/5
        [HttpGet("{userId}")]
<<<<<<< HEAD
        public string Get(string userId)
=======
        public async Task<IActionResult> GetAsync(string userId)
>>>>>>> d87440f271a20753fb3dcc86b1fc3fe8bebdccb3
        {
            if (String.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

<<<<<<< HEAD
            var UserSqlQuery = "SELECT user.DETECTED_FOOD, user.PHOTO_URL FROM user_food_tbl as user WHERE user_id = @userId AND DETECTED_FOOD_SOURCE = \"AzureMachineLearning\"";
            var FoodFactsSqlQuery = "select facts.FACT_NAME, facts.FACT_VALUE, facts.FACT_UNIT from food_facts_tbl as facts where FOOD_NAME = @foodName AND FACT_NAME in (\"Fat\", \"Calories\", \"Protein\", \"Carbohydrate\", \"Fiber\", \"Sugars\")";
            MySql.Data.MySqlClient.MySqlCommand Command = new MySql.Data.MySqlClient.MySqlCommand(UserSqlQuery, MySqlConnection);
            Command.Parameters.AddWithValue("@userId", userId);
            MySql.Data.MySqlClient.MySqlDataReader SqlReader = Command.ExecuteReader();
            List<UserFoodData> results = new List<UserFoodData>();
            while (SqlReader.Read())
            {
                var userFoodInfo = new UserFoodData();
                userFoodInfo.foodName = (string) SqlReader["DETECTED_FOOD"];
                userFoodInfo.userId = userId;
                userFoodInfo.imageUrl = (string)SqlReader["PHOTO_URL"];
                results.Add(userFoodInfo);
            }
            SqlReader.Close();
            //Command.CommandText = FoodFactsSqlQuery;
            foreach (UserFoodData item in results)
            {
                MySql.Data.MySqlClient.MySqlCommand FoodFactCommand = new MySql.Data.MySqlClient.MySqlCommand(FoodFactsSqlQuery, MySqlConnection);
                //Command.Parameters.Clear();
                FoodFactCommand.Parameters.AddWithValue("@foodName", item.foodName);
                MySql.Data.MySqlClient.MySqlDataReader FoodFactReader = FoodFactCommand.ExecuteReader();
                var NutritionFacts = new FoodFacts();
                while (FoodFactReader.Read())
                {
                    string FactName = (string) FoodFactReader["FACT_NAME"];
                    switch (FactName)
                    {
                        case "Fat":
                            NutritionFacts.fat = (string)FoodFactReader["FACT_VALUE"] + (string)FoodFactReader["FACT_UNIT"];
                            break;
                        case "Protein":
                            NutritionFacts.protein = (string)FoodFactReader["FACT_VALUE"] + (string)FoodFactReader["FACT_UNIT"];
                            break;
                        case "Carbohydrate":
                            NutritionFacts.carbohydrate = (string)FoodFactReader["FACT_VALUE"] + (string)FoodFactReader["FACT_UNIT"];
                            break;
                        case "Calories":
                            NutritionFacts.calories = (string)FoodFactReader["FACT_VALUE"] + (string)FoodFactReader["FACT_UNIT"];
                            break;
                        case "Sugars":
                            NutritionFacts.sugar = (string)FoodFactReader["FACT_VALUE"] + (string)FoodFactReader["FACT_UNIT"];
                            break;
                        case "Fiber":
                            NutritionFacts.fiber = (string)FoodFactReader["FACT_VALUE"] + (string)FoodFactReader["FACT_UNIT"];
                            break;
                    }
                }
                item.nutrition = NutritionFacts;
                FoodFactReader.Close();
            }
            MySqlConnection.Close();
=======
            var foodList = await Database.GetFoodByUserAsync(userId);
            foreach (var foodItem in foodList)
            {
                foodItem.nutrition = await Database.GetFoodFactsAsync(foodItem.foodName);
            }
>>>>>>> d87440f271a20753fb3dcc86b1fc3fe8bebdccb3

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
