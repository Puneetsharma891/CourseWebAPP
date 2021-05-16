using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWebAPI.Models
{
    public class CourseService
    {
        private List<Course> list;

        public CourseService()
        {
            Course c1 = new Course
            {
                CourseId = "C01",
                CourseName = "English",
                Duration = 2.5,
                InstructorName = "Puneet"
            };
            Course c2 = new Course
            {
                CourseId = "C02",
                CourseName = "Math",
                Duration = 21.5,
                InstructorName = "Akhil"
            };
            Course c3 = new Course
            {
                CourseId = "C03",
                CourseName = "Hindi",
                Duration = 12.5,
                InstructorName = "Kanu"
            };
            Course c4 = new Course
            {
                CourseId = "C04",
                CourseName = "Science",
                Duration = 1.5,
                InstructorName = "Parul"
            };

            list = new List<Course>();
            list.Add(c1);
            list.Add(c2);
            list.Add(c3);
            list.Add(c4);
        }

        public IEnumerable<Course> GetCourses()
        {
            return list;
        }

        public Course GetCourse(string id)
        {
            return list.First(z=>z.CourseId.Equals(id));
        }

        public void AddCourse(Course course)
        {
            list.Add(course);
        }
    }
}
