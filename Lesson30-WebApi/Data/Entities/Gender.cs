using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lesson30_WebApi.Data.Entities
{
    public class Gender:ISoftDelete
    {
        public int Id { get; set; }
        [StringLength(maximumLength:20)]
        public string Name { get; set; }
        public List<Student> Students { get; set; }
        public bool IsDeleted { get; set; }
    }
}
