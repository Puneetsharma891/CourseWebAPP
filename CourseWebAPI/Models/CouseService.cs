using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWebAPI.Models
{
    public class CourseService
    {
        SqlConnection sqlCon;
        string _connectionString;
        string tabelName;
        public CourseService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("SQLString");
            tabelName = _configuration["TableName"];
        }

        public void OpenConnection()
        {
            sqlCon = new SqlConnection(_connectionString);
            sqlCon.Open();
        }
        public IEnumerable<Course> GetCourses()
        {
            List<Course> courseList = new List<Course>();
            DataTable dt = new DataTable();
            OpenConnection();
            string query = $"select CourseId,CourseName,Duration,InstructorName from [dbo].[{tabelName}]";
            SqlCommand cmd = new SqlCommand(query, sqlCon);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(dt);

            foreach(DataRow row in dt.Rows)
            {
                Course course = new Course
                {
                    CourseId = row[0].ToString(),
                    CourseName = row[1].ToString(),
                    Duration = int.Parse(row[2].ToString()),
                    InstructorName = row[3].ToString(),
                };
                courseList.Add(course);
            }
            return courseList;
        }

        public Course GetCourse(string id)
        {
            List<Course> courseList = new List<Course>();
            DataTable dt = new DataTable();
            OpenConnection();
            SqlCommand cmd = new SqlCommand
                ($"select CourseId,CourseName,Duration,InstructorName from [dbo].[{tabelName}] where CourseId='{id}'", sqlCon);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(dt);

            DataRow row = dt.Rows[0];
            Course course = new Course
            {
                CourseId = row[0].ToString(),
                CourseName = row[1].ToString(),
                Duration = int.Parse(row[2].ToString()),
                InstructorName = row[3].ToString(),
            };
            return course;
        }

        public void AddCourse(Course course)
        {
            
            List<Course> courseList = new List<Course>();
            DataTable dt = new DataTable();
            OpenConnection();
            string query = $"INSERT INTO {tabelName} VALUES('{course.CourseId}','{course.CourseName}',{course.Duration},'{course.InstructorName}');";
            SqlCommand cmd = new SqlCommand(query, sqlCon);
            cmd.ExecuteNonQuery();
        }
    }
}
