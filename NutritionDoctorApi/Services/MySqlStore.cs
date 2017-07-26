using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NutritionDoctorApi.Model;
using System.Collections.Generic;

namespace NutritionDoctorApi.Services
{
    public class MySqlStore
    {
        private string ConnectionString = "database=pingandb;data source=us-cdbr-azure-east-c.cloudapp.net;user id=b8639718fe5ad6;password=2cd7b667";
        private const string FoodFactTable = "food_facts_tbl";
        private const string UserFoodTable = "user_food_tbl";

        public async Task<IList<FoodFact>> GetFoodFactsAsync(string foodName)
        {
            var sqlCommand = $"SELECT * FROM {FoodFactTable} WHERE FOOD_NAME = '{foodName}'";

            Func<MySqlDataReader, Task<IList<FoodFact>>> func = async (reader) =>
            {
                var result = new List<FoodFact>();

                while (await reader.ReadAsync())
                {
                    var fact = new FoodFact
                    {
                        FactName = await reader.GetFieldValueAsync<string>(2),
                        FactValue = await reader.GetFieldValueAsync<string>(3),
                        FactUnit = await reader.GetFieldValueAsync<string>(4)
                    };
                    result.Add(fact);
                }

                return result;
            };

            return await ExecuteQueryAsync(sqlCommand, func);
        }

        public async Task<IList<UserFoodData>> GetFoodByUserAsync(string userId)
        {
            var sqlCommand = $"SELECT * FROM {UserFoodTable} WHERE USER_ID = '{userId}' AND DETECTED_FOOD_SOURCE = 'AzureMachineLearning'";

            Func<MySqlDataReader, Task<IList<UserFoodData>>> func = async (reader) =>
            {
                var result = new List<UserFoodData>();

                while (await reader.ReadAsync())
                {
                    var foodData = new UserFoodData 
                    {
                        userId = await reader.GetFieldValueAsync<string>(1),
                        imageUrl = await reader.GetFieldValueAsync<string>(2),
                        foodName = await reader.GetFieldValueAsync<string>(3)
                    };
                    result.Add(foodData);
                }

                return result;
            };

            return await ExecuteQueryAsync(sqlCommand, func);
        }

        private async Task<T> ExecuteQueryAsync<T>(string sqlCommand, Func<MySqlDataReader, Task<T>> parseFunc)
        {
            T result = default(T);
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                using (var command = new MySqlCommand(sqlCommand, connection))
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        result = await parseFunc(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                await Console.Error.WriteAsync(ex.Message);
            }

            return result;
        }
    }
}
