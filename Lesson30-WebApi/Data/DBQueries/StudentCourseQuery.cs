using System.ComponentModel.DataAnnotations;
using System;

namespace Lesson30_WebApi.Data.DBQueries
{
    public class StudentCourseQuery
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string CourseName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


    }
}
