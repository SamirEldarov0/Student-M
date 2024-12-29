using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lesson30_WebApi.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(15)]
        public string Name { get; set; }
        [Required]
        [MinLength(5)]
        public string Surname { get; set; }
        [Required]
        [Range(100,1000)]
        public double Salary { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        [Range(1,3)]
        public int GenderId { get; set; }

    }
    public class StudentValidator:AbstractValidator<StudentModel>
    {
        public StudentValidator()
        {
            RuleFor(x=>x.Name).Length(1,15).NotNull().NotEmpty();
            RuleFor(x => x.Surname).Length(min: 5, max: 25).NotNull();
            RuleFor(x => x.Salary).InclusiveBetween(100, 1000);
            RuleFor(x => x.GenderId).InclusiveBetween(1, 3);
            RuleFor(x => x.GenderId).GreaterThan(1);
        }
    }
}
