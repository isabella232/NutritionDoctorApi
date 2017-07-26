using NutritionDoctorApi.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;

namespace NutritionDoctorApi.Model
{
    public struct FoodFact
    {
        public string FactName { get; set; }
        public string FactValue { get; set; }
        public string FactUnit { get; set; }
    }
}
