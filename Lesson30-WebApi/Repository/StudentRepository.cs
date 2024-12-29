using Lesson30_WebApi.Data.DAL;
using Lesson30_WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lesson30_WebApi.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentDbContext _context;

        public StudentRepository(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<Student> Add(Student entity)
        {
            await _context.Students.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Student> Delete(int id)
        {
            Student student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return student;
        }

        //public async Task<bool> Delete(Student entity)
        //{
        //    Student student = await _context.Students.FirstOrDefaultAsync(x => x.Id == entity.Id);
        //    _context.Students.Remove(student);
        //    _context.SaveChanges();
        //    return true;
        //    //throw new System.NotImplementedException();
        //}

        public async Task<Student> Get(int id)
        {
            //Student student1 =await _context.Students.FindAsync(id);
            Student student =await _context.Students.Include(x=>x.StudentCourses).FirstOrDefaultAsync(x => x.Id == id);
            return student;
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            return await _context.Students.Include(x=>x.Gender).ToListAsync();
            //throw new System.NotImplementedException();
        }

        public async Task<Student> Update(Student entity)
        {
            var student = await _context.Students.FindAsync(entity);
            this._context.Entry(student).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return student;
            //throw new System.NotImplementedException();
        }
    }
}
