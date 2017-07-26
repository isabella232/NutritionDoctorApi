using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NutritionDoctorApi.Model
{
    public enum FoodFactType
    {
        Calories,
        Fat,
        Protein,
        Carbohydrate,
        Fiber,
        Sugars,
        [Description("Vitamin A")]
        Vitamin_A,
        [Description("Vitamin C")]
        Vitamin_C,
        Thiamin,
        Riboflavin,
        Niacin,
        Folate,
        [Description("Vitamin E (alpha-tocopherol)")]
        Vitamin_E,
        Cholesterol,
        Calcium,
        Iron,
        Magnesium,
        Phosphorus,
        Potassium,
        Sodium,
        Zinc,
    }
}
