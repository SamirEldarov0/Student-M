using Lesson30_WebApi.Data.Entities;
using Lesson30_WebApi.Repository;
using System.Threading.Tasks;

namespace Lesson30_WebApi.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Student,int> StudentRepository { get; set; }
        //public IGenericRepository<Course,int> CourseRepository { get; set; }//
        public ICourseRepository CourseRepository { get; set; }
        public Task Commit();
    }
}
