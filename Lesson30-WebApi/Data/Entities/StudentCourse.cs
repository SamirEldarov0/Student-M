using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson30_WebApi.Data.Entities
{
    public class StudentCourse
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //[Key]
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        //[Key]
        public int StudentId { get; set; }
        //[ForeignKey(nameof(StudentTestId))]
        public Student Student { get; set; }
        //[ForeignKey(nameof(Course))]
        //[Key]
        public int CourseId { get; set; }
        //[ForeignKey(nameof(CourseXId))]
        public Course Course { get; set; }


    }
}
