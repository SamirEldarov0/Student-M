using System.ComponentModel.DataAnnotations;

namespace Lesson30_WebApi.Data.Entities
{
    public class Checking
    {
        [Key]
        public int IdTest { get; set; }
        [MinLength(10)]
        public string Name { get; set; }

    }
}
