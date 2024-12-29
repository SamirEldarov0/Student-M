using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson30_WebApi.Data.Entities
{
    public class Course:BaseEntity<int>
    {
        //[Key]
        public override int Id { get; set; }
        //[StringLength(maximumLength:40)]
        //[StringLength(25,MinimumLength =10)]
        
        [Required]
        public string Name { get; set; }
        [Column("CreatedDate",TypeName ="datetime")]
        public DateTime? CreationTime { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
    }
}
