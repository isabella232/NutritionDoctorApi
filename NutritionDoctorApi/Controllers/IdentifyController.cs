using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NutritionDoctorApi.Model;

namespace NutritionDoctorApi.Controllers
{
    [Route("api/user/{userId}/[controller]")]
    public class IdentifyController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post(IdentifyRequest request)
        {
            // 1. generate "job" id
            // 2. save image to blob
            // 3. save job to queue
            // 4. return 201 with job
        }
    }
}
