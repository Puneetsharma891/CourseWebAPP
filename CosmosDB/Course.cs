using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDB
{
    public class Course
    {
        public Course()
        {
        }

        public Course(string _id, string _CourseId, string _CourseName, int _Duration, string _InstructorName)
        {
            id = _id;
            CourseName = _CourseName;
            CourseId = _CourseId;
            Duration = _Duration;
            InstructorName = _InstructorName;
        }
        public string id { get; set; }
        public string CourseName { get; set; }
        public string CourseId { get; set; }

        public double Duration { get; set; }

        public string InstructorName { get; set; }

    }
}
