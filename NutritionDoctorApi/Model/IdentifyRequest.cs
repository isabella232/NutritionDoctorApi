using System.Collections.Generic;

namespace NutritionDoctorApi.Model
{
    public class IdentifyRequest
    {
        public string userId { get; set; }
        public string imageData { get; set; }
    }
    public class UserFoodData
    {
        public string userId { get; set; }
        public string imageUrl { get; set; }
        public string foodName { get; set; }
        public IList<FoodFact> nutrition { get; set; }
    }
}
