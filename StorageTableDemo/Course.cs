using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageTableDemo
{
    public class Course : TableEntity
    {
        public Course()
        {
        }

        public Course(string CourseId, string CourseName, int _Duration, string _InstructorName) : base(CourseId, CourseName)
        {
            PartitionKey = CourseName;
            RowKey = CourseId;
            Duration = _Duration;
                InstructorName = _InstructorName;
        }

        public double Duration { get; set; }

        public string InstructorName { get; set; }

    }
}
