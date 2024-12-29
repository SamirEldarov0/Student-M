using Lesson30_WebApi.Data.DBQueries;
using Lesson30_WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lesson30_WebApi.Data.DAL
{
    public class StudentDbContext:DbContext
    {
        private static readonly MethodInfo ConfigureGlobalFiltersMethodInfo=
            typeof(StudentDbContext).GetMethod(nameof(ConfigureGlobalFiltersMethodInfo),BindingFlags.Instance | BindingFlags.NonPublic);
        //bu method haqqinda informasiyani ozumde saxlayitam,method instance ve NonPublicdimi yeni protected ve ya private olmali
        public StudentDbContext(DbContextOptions<StudentDbContext> options):base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Checking> Checkings { get; set; }
        public DbSet<StudentCourseQuery> StudentCourseQueries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //modelBuilder.Entity<Course>().Property(x => x.CreationTime).HasDefaultValueSql("getdate()");//bazada insert eldiyi vaxdi goturur
            //modelBuilder.Entity<Student>().ToTable("tblStudnet");
            //modelBuilder.Entity<Course>().ToTable("tblCourse");
            //modelBuilder.Entity<Gender>().ToTable("tblGender");
            //modelBuilder.Entity<StudentCourse>().ToTable("tblStudentCourse");
            //modelBuilder.Entity<StudentCourse>().HasKey(x => new{ x.StudentId, x.CourseId });500 error prime key yoxdu bunlar primary key
            modelBuilder.Entity<StudentCourseQuery>().HasNoKey();

            //modelBuilder.Entity<Student>(s =>
            //{
            //    s.HasKey(k => k.Id);
            //    s.ToTable("tblStudent");
            //    s.HasQueryFilter(x => x.Id == 1);
            //    s.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UIX_Students_Name");//Emailler uniq olur user-de nese
            //    s.Property(s => s.Name).HasMaxLength(30);
            //    s.Property(s => s.Surname).HasMaxLength(25);
            //    s.HasOne(x => x.Gender).WithMany(x => x.Students).HasForeignKey(x=>x.GenderId).HasConstraintName("FK_Students_Genders_GenderId");
            //});
            //modelBuilder.Model.GetEntityTypes

            #region GlobalFilters
            //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    ConfigureGlobalFiltersMethodInfo.
            //        MakeGenericMethod(entityType.ClrType)//method genericdi ve generici ancaq runtime-da belli olur ve entitytipin ClrType-onun tipini m edir  type-da 
            //        //typeda reflectionda istifade olunur 
            //        //reflection- runtime-da  c#da classin icinde bezi seyleri sorguluyub onlari avtomatik cagira ,deyisenin qiymetini deyise bilersizn
            //        .Invoke(this,new object[] {modelBuilder});
            //}
            #endregion

            ConfigureGlobalFilters<Student>(modelBuilder);
            ConfigureGlobalFilters<Course>(modelBuilder);
            ConfigureGlobalFilters<Gender>(modelBuilder);
            ConfigureGlobalFilters<StudentCourse>(modelBuilder);


            //foreach (var item in modelBuilder.Model.GetEntityTypes())
            //{
            //    //ConfigureGlobalFilters(modelBuilder, item); basqa movzulara girecek,problem o generic methodu cagirmaq ucun entity-in tipi belli olmali

            //}
        }
        protected void ConfigureGlobalFilters<TEntity> (ModelBuilder modelBuilder/*,IMutableEntityType entityType*/) where TEntity : class
        {
            if (ShouldFilterEntity<TEntity>())
            {
                var filterExpression = CreateFilterExpression<TEntity>();
                if (filterExpression!=null)
                {
                    modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
                }
            }
        }

        protected virtual bool ShouldFilterEntity<TEntity>(/*,IMutableEntityType entityType*/) where TEntity : class
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))//entitim ISoftDelete den toruyubse,reflection axridadanisacam
            {
                return true;
            }
            return false;
        }

        protected virtual Expression<Func<TEntity,bool>> CreateFilterExpression<TEntity>() where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> softDeleteFilter = e => ((ISoftDelete)e).IsDeleted;
                expression= softDeleteFilter;
            }
            return expression;
        }
    }
}
