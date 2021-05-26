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
            List<Course> courses = new List<Course>();

            courses.Add(new Course("1", "C01", "Hindi", 12, "Puneet"));
            courses.Add(new Course("2", "C02", "English", 12, "Parul"));
            courses.Add(new Course("3", "C03", "Hindi", 12, "Puneet"));
            courses.Add(new Course("4", "C04", "Hindi", 12, "Puneet"));
            courses.Add(new Course("5", "C05", "Math", 12, "Puneet"));
            courses.Add(new Course("6", "C06", "science", 12, "Puneet"));

            return courses;
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
