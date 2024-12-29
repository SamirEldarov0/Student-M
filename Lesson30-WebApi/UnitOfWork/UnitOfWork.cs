using Lesson30_WebApi.Data.DAL;
using Lesson30_WebApi.Data.Entities;
using Lesson30_WebApi.Repository;
using System.Threading.Tasks;

namespace Lesson30_WebApi.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StudentDbContext _context;

        public IGenericRepository<Student, int> StudentRepository { get; set; }
        //public IGenericRepository<Course, int> CourseRepository { get; set; }
        public ICourseRepository CourseRepository { get; set; }
        public UnitOfWork(StudentDbContext context)
        {
            StudentRepository =new EFGenericRepository<Student, int>(context);
            //CourseRepository = new EFGenericRepository<Course, int>(context);
            CourseRepository = new CourseRepository(context);
            _context = context;
        }
        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
