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

        public async Task<IList<FoodFact>> GetFoodFactsAsync(string foodName)
        {
            var foodFacts = new List<FoodFact>();

            try
            {
                var sqlCommand = $"SELECT * FROM {FoodFactTable} WHERE FOOD_NAME = '{foodName}'";

                using (var connection = new MySqlConnection(ConnectionString))
                using (var command = new MySqlCommand(sqlCommand, connection))
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            var fact = new FoodFact
                            {
                                FactName = await reader.GetFieldValueAsync<string>(2),
                                FactValue = await reader.GetFieldValueAsync<string>(3),
                                FactUnit = await reader.GetFieldValueAsync<string>(4)
                            };
                            foodFacts.Add(fact);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Console.Error.WriteAsync(ex.Message);
            }

            return foodFacts;
        }
    }
}
