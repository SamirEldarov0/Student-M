using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson30_WebApi.Data.Entities
{
    public class Student:BaseEntity<int>,ISoftDelete
    {
        [Key]
        //public int Id1 { get; set; }
        public override int Id { get; set; }
        public string Name { get; set; }
        [StringLength(maximumLength: 40)]
        public string Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public double Salary { get; set; }
        //[ForeignKey(nameof(Gender))]
        public int? GenderId { get; set; }
        //public int GenderId { get; set; } Bu halda EF ozu basa dusur

        //public Gender Gender { get; set; }//navigation property
        public Gender Gender { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
        public bool IsDeleted { get; set; }
    }
}
