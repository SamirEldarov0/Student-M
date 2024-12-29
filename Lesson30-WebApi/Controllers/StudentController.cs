using Lesson30_WebApi.Configurations;
using Lesson30_WebApi.Data.DAL;
using Lesson30_WebApi.Data.Entities;
using Lesson30_WebApi.Helpers;
using Lesson30_WebApi.Models;
using Lesson30_WebApi.Repository;
using Lesson30_WebApi.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson30_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        
        private readonly List<StudentModell> _students = new List<StudentModell>();
        private readonly ILogger<StudentController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Student, int> _genericStudentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISingletonOperation _singletonOperation;
        private readonly ITransientOperation _transientOperation;
        private readonly IScopedOperation _scopedOperation;
        private readonly PositionOptions _positionOptions;
        private readonly StudentDbContext _context;

        public StudentController(ILogger<StudentController> logger,IUnitOfWork unitOfWork,IGenericRepository<Student,int> genericStudentRepository,IStudentRepository studentRepository,StudentDbContext context,IOptions<PositionOptions> positionAccessor,IConfiguration configuration,IServiceProvider serviceProvider,ISingletonOperation singletonOperation,ITransientOperation transientOperation,IScopedOperation scopedOperation)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _genericStudentRepository = genericStudentRepository;
            _studentRepository = studentRepository;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _singletonOperation = singletonOperation;
            _transientOperation = transientOperation;
            _scopedOperation = scopedOperation;
            _positionOptions = positionAccessor.Value;
            _context = context;
        }
        [HttpGet("Guids")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult GetGuids()
        {
            var user = HttpContext.User;
            var IsInAdminRole = user.IsInRole("Admin");
            var transientOp1 = (ITransientOperation)_serviceProvider.GetService(typeof(ITransientOperation));
            var scopedOp1 = (IScopedOperation)_serviceProvider.GetService(typeof(IScopedOperation));
            var scoped2 = (IScopedOperation)_serviceProvider.GetService(typeof(IScopedOperation));
            var transient2 = (ITransientOperation)_serviceProvider.GetService(typeof(ITransientOperation));

            var operations = new
            {
                Singleton = _singletonOperation.Id,
                Transient1 = transientOp1.Id,
                Scoped1 = scopedOp1.Id,
                Scoped2 = scoped2.Id,
                Transient2 = transient2.Id
            };
            return Ok(operations);
        }


        [HttpGet("getcourses")]
        public async Task<IActionResult> GetAllCourses()
        {
            //var courses =await _context.Courses.ToListAsync();
            var courses = await _unitOfWork.CourseRepository.GetAllList();
            //var courses = _unitOfWork.CourseRepository.FindByName("C#");

            return Ok(courses);
        }

        [HttpGet]
        [Route("all")]
        //[MyAuthorized]
        //[Authorize("AdminOnly")]
        public async Task<object> GetAll()
        {
            var context = HttpContext;
            var user = HttpContext.User;
            _logger.LogInformation("Request accepted at {date}", DateTime.Now);
            //var query = _genericStudentRepository.GetAll();//getalllistde olar
            var query = _unitOfWork.StudentRepository.GetAll();

            var result= await query.ToListAsync();
            _logger.LogWarning($"Request succesfully completed at  : {DateTime.Now}");
            return await query.ToListAsync();
            //var query = await _studentRepository.GetAll();
            //return query;

            //var students = _context.Students.Include(x => x.Gender).Include(x => x.StudentCourses).ThenInclude(x => x.Course)
            //    .Select(x => new
            //    {
            //        x.Name,
            //        x.Surname,
            //        x.DateOfBirth,
            //        GenderName = x.Gender.Name,
            //        Courses = x.StudentCourses.Select(c => new//.Where(y => y.StudentId == x.Id)
            //        {
            //            c.Course.Name,
            //            c.Course.CreationTime,
            //            c.StartDate//studentcourse
            //        })
            //    });


            //var sqlString=students.ToQueryString();
            //return await students.ToListAsync();
            //return students;//anonim tipolduguna gore 
        }
        //[HttpGet("studentcoursereport")]
        //public async Task<object> GetReport()
        //{
        //    //joinleri yaziriq
        //    //linq query 
        //var query = from sc in _context.StudentCourses
        //            join s in _context.Students on sc.StudentId equals s.Id
        //            join c in _context.Courses on sc.CourseId equals c.Id
        //            join g in _context.Genders on s.GenderId equals g.Id
        //            select new
        //            {
        //                s.Name,
        //                s.Surname,
        //                GenderName=g.Name,
        //                s.DateOfBirth,
        //                CourseName = c.Name,
        //                sc.StartDate,
        //                sc.EndDate
        //            };
        //    //await _context.Database.ExecuteSqlRawAsync("insert into Genders \r\nvalues('Unknown')");

        //    var result = await _context.StudentCourseQueries.FromSqlRaw(@"select s.Name,s.Surname,s.DateOfBirth,c.Name CourseName,sc.StartDate,sc.EndDate from StudentCourses sc
        //        join Students s
        //        on sc.StudentId=s.Id
        //        join Courses c
        //        on c.Id=sc.CourseId").ToListAsync();
        //    //var sqlString = query.ToQueryString();

        //    return result;

        //    //return Ok(await query.ToListAsync());
        //}


        //[HttpGet("genders")]
        //public IActionResult GetGenders()
        //{
        //    var genders = _context.Genders.Include(x=>x.Students)
        //        .Select(x => new
        //        {
        //            x.Name,
        //            Students=x.Students.Select(y => new
        //            {
        //                y.Surname,
        //                y.Name, y.DateOfBirth
        //            })
        //        })
        //        .ToListAsync();
        //    return Ok(genders);
        //}

        [HttpPost("create")]
        //[FromBody],[FromQuery]string name
        public async Task<IActionResult> Create([FromBody]StudentModel studentModel,[FromServices]IOptions<ApiBehaviorOptions> options)
        {
            Student student = new Student()
            {
                Name=studentModel.Name,
                Surname= studentModel.Surname,
                DateOfBirth =studentModel.DateOfBirth,
                Salary=studentModel.Salary,
                GenderId=studentModel.GenderId
            };
            await _unitOfWork.StudentRepository.Add(student);
            await _unitOfWork.Commit();//icerisinde butun deyisiklikler yeni butun repositorlara aid deyisikler commit olsun
            //await _genericStudentRepository.Add(student);
            //await _genericStudentRepository.Commit();//StudentRepositori oz icerisindeki contextin instansini commit edir
            //await _studentRepository.Add(student);
            //await _context.Students.AddAsync(student);
            //await _context.SaveChangesAsync();
            options.Value.InvalidModelStateResponseFactory(ControllerContext);
            return StatusCode(201, student);
            //_students.Add(student);
            //return student;
        }
        [HttpPut("update")]
        public async Task<IActionResult> Edit(int id,Student newstudent)
        {
            var student =await _unitOfWork.StudentRepository.Find(id);

            student.Name = newstudent.Name;
            student.Salary = newstudent.Salary;
            await _unitOfWork.StudentRepository.Update(student);
            await _unitOfWork.Commit();
            ////Student student =await _studentRepository.Get(id);
            //Student student =await _genericStudentRepository.Find(id);
            //if (student==null)
            //{
            //    return NotFound();
            //}
            //student.Name = newstudent.Name;
            //student.Salary = newstudent.Salary;
            //await _genericStudentRepository.Update(student);
            //await _studentRepository.Update(student);

            //var student =await _students.FirstOrDefaultAsync(x => x.Id == id);
            //var student =await _context.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            //_context.Entry(student).State = EntityState.Modified;
            //_context.Attach(student);//normalda hecne yazmiyanda EF yazdigini savechange edir

            //if (student==null)
            //{
            //    return NotFound();
            //}
            //student.Name=newstudent.Name;
            //await _context.SaveChangesAsync();
            return Ok(204);//onda student qayidib
        }
        [HttpDelete("delete")]

        public async Task<Student> Delete(int id)//evvel Student -> StudentModel idi
        {
            //Student student = await _studentRepository.Get(id);
            //var deletedStudent = await _studentRepository.Delete(id);
            try
            {
                _logger.LogInformation($"Request accepted succesfully to delete student with id {id}");
                Student student = await _unitOfWork.StudentRepository.Find(id);
                _logger.LogDebug("Student is fetched from db scsflly");
                var deleteStudent = _unitOfWork.StudentRepository.Delete(student);

                await _unitOfWork.Commit();
                _logger.LogDebug("Student is deleted from db succesfully and transaction committed");
                _logger.LogInformation($"Request is completed to delete the student {id}");
                return student;

            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error occured when deleting the student {id}",id);
                throw;
            }
           
            //Student student = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);
            //_context.Students.Remove(student);
            //await _context.SaveChangesAsync();
            //StudentModel student = await _students.FirstOrDefaultAsync(x => x.Id == id);           
            //_students.Remove(student);
            //return student;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var request = Request;
            var user = User;
            var context = HttpContext;
            //var student = await _context.Students.Include(x=>x.StudentCourses).FirstOrDefaultAsync(x => x.Id == id);
            //var student1 =await _genericStudentRepository.Find(id);
            //var student = _genericStudentRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            var student =await _unitOfWork.StudentRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (student==null)
            {
                return NotFound();
            }
            //Student student =await _studentRepository.Get(id);//Artiq birbasa ef-den asili deyilem
            return Ok(student);
        }

        [HttpGet("config")]
        public IActionResult GetConfigurations()
        {
            var configurations = new//anonim tip 
            {
                GeneralAPiKey = _configuration["ApiKey"],
                SMSApi = _configuration["SMSApi"],
                FromNumber = _configuration["SMSApi:FromNumber"],
                SMSApiKey = _configuration["SMSApi:SMSApiKey"],
                PositionOptions = _positionOptions
            };
            var json=JsonConvert.SerializeObject(configurations);
            return Ok(configurations);
        }
    }
}
