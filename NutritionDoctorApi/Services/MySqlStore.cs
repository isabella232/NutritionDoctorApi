using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NutritionDoctorApi.Model;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NutritionDoctorApi.Services
{
    public class MySqlStore
    {
        public static IConfigurationRoot Configuration { get; set; }

        private string ConnectionString = "";
        private string FoodFactTable = "";
        private string UserFoodTable = "";

        public MySqlStore()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            ConnectionString = $"{Configuration["mysql_connstring"]}";
            FoodFactTable = $"{Configuration["mysql_foodtbl"]}";
            UserFoodTable = $"{Configuration["mysql_usertbl"]}";
        }

        public async Task<Nutrition> GetFoodFactsAsync(string foodName)
        {
            var sqlCommand = $"SELECT * FROM {FoodFactTable} WHERE FOOD_NAME = '{foodName}'";

            Func<MySqlDataReader, Task<Nutrition>> func = async (reader) =>
            {
                var nutrition = new Nutrition();
                while (await reader.ReadAsync())
                {
                    var name = await reader.GetFieldValueAsync<string>(2);
                    var value = await reader.GetFieldValueAsync<string>(3);
                    var unit = await reader.GetFieldValueAsync<string>(4);

                    var propName = Regex.Replace(name, @"\(.*?\)", string.Empty);
                    propName = Regex.Replace(propName, @"\s+", string.Empty);

                    Type type = nutrition.GetType();
                    PropertyInfo prop = type.GetProperty(propName);
                    prop?.SetValue(nutrition, new FoodFact { FactValue = value, FactUnit = unit });

                    if (prop == null)
                    {
                        Console.WriteLine($"Unable to parse {propName}");
                    }
                }

                return nutrition;
            };

            return await ExecuteQueryAsync(sqlCommand, func, true);
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
                        foodName = await reader.GetFieldValueAsync<string>(3),
                        createdDateTime = await reader.GetFieldValueAsync<DateTime>(5),
                    };
                    result.Add(foodData);
                }

                return result;
            };

            return await ExecuteQueryAsync(sqlCommand, func);
        }

        private async Task<T> ExecuteQueryAsync<T>(string sqlCommand, Func<MySqlDataReader, Task<T>> parseFunc, bool tableCache = false)
        {
            T result = default(T);
            try
            {
                if (tableCache)
                {
                    ConnectionString += ";table cache=true";
                }

                using (var connection = new MySqlConnection(ConnectionString))
                using (var command = new MySqlCommand(sqlCommand, connection))
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        result = await parseFunc(reader);
                    }
                    
                    connection.Close();
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
