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
        public FoodFacts nutrition { get; set; }
    }

    public class FoodFacts
    {
        public string calories { get; set; }
        public string fat { get; set; }
        public string protein { get; set; }
        public string carbohydrate { get; set; }
        public string fiber { get; set; }
        public string sugar { get; set; }
    }
}
