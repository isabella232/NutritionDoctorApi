using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NutritionDoctorApi.Model;
using NutritionDoctorApi.Services;
using System.Net;
using System.Net.Http;

namespace NutritionDoctorApi.Controllers
{
    [Route("api/user/[controller]")]
    public class IdentifyController : Controller
    {
        // GET api/user/identify
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/user/identify/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/user/identify
        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync([FromBody] IdentifyRequest request)
        {
            // 1. generate "job" id
            Guid guid = Guid.NewGuid();
            string jobid = guid.ToString();

            // 2. save image to blob
            BlobService blobService = new BlobService();
            string imageBlobUri = await blobService.UploadImageToBlob(request.userId, request.imageData);

            // 3. save job to queue
            // needs to send job id and image URL to a method that add job to queue
            // string jobId
            // string imageBlobUri

            // 4. return 201 with job
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}
