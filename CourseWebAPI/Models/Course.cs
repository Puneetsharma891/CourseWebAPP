using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWebAPI.Models
{
    public class Course
    {
        public string CourseId { get; set; }

        public string CourseName { get; set; }

        public double Duration { get; set; }

        public string InstructorName { get; set; }

        
    }
}
