using Lesson30_WebApi.Data.DAL;
using Lesson30_WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson30_WebApi.Repository
{
    public interface ICourseRepository:IGenericRepository<Course,int>
    {
        Task<Course> FindByName(string name);
    }
    public class CourseRepository:EFGenericRepository<Course,int>, ICourseRepository
    {
        private readonly StudentDbContext _context;
        public CourseRepository(StudentDbContext context):base(context)
        {
            _context = context;
        }
        public async Task<Course> FindByName(string name)
        {
            var course =await _context.Set<Course>().Where(x => x.Name.ToUpper().Contains(name)).FirstOrDefaultAsync();
            return course;

        }
    }

}
