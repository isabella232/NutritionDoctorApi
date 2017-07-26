namespace NutritionDoctorApi.Model
{
    public class Nutrition
    {
        public FoodFact Calories { get; set; }
        public FoodFact Fat { get; set; }
        public FoodFact Protein { get; set; }
        public FoodFact Carbohydrate { get; set; }
        public FoodFact Fiber { get; set; }
        public FoodFact Sugars { get; set; }
        public FoodFact VitaminA { get; set; }
        public FoodFact VitaminC { get; set; }
        public FoodFact Thiamin { get; set; }
        public FoodFact Riboflavin { get; set; }
        public FoodFact Niacin { get; set; }
        public FoodFact Folate { get; set; }
        public FoodFact VitaminE { get; set; }
        public FoodFact Cholesterol { get; set; }
        public FoodFact Calcium { get; set; }
        public FoodFact Iron { get; set; }
        public FoodFact Magnesium { get; set; }
        public FoodFact Phosphorus { get; set; }
        public FoodFact Potassium { get; set; }
        public FoodFact Sodium { get; set; }
        public FoodFact Zinc { get; set; }
    }

    public class FoodFact
    {
        public string FactValue { get; set; }
        public string FactUnit { get; set; }
    }
}
