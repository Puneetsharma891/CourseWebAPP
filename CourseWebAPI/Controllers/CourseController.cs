using CourseWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CourseWebAPI.Controllers
{
    [ApiController]
    [Route("/api/Course")]
    public class CourseController : Controller
    {
        CourseService service;
       
        public CourseController(CourseService _service)
        {
            service = _service;
        }

        [HttpGet]
        public IEnumerable<Course> GetCourses()
        {
            return service.GetCourses();
        }

        [HttpGet]
        [Route("{id}")]
        public Course GetCourse(string id)
        {
            return service.GetCourse(id);
        }

        [HttpPost]
        public IActionResult AddCourse()
        {
            string body = string.Empty;
            using (StreamReader rd = new StreamReader(Request.Body))
            {
                body = rd.ReadToEnd();
            }
            Course course = JsonConvert.DeserializeObject<Course>(body);

            service.AddCourse(course);

            return new OkResult();
        }

        [HttpGet]
        [Route("ThrowError")]
        public IActionResult ThrowError()
        {
            IActionResult rep = new StatusCodeResult(500);

            return rep;
        }
    }
}
