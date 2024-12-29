using Lesson30_WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lesson30_WebApi.Data.EntityConfigurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {

            //builder.HasKey(k => k.Id);
            //builder.ToTable("tblStudent");
            //builder.HasQueryFilter(x => x.Id == 1);          
            //builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UIX_Students_Name");//Emailler uniq olur user-de nese
            //builder.Property(s => s.Name).HasMaxLength(30);
            //builder.Property(s => s.Surname).HasMaxLength(25);
            //builder.HasOne(x => x.Gender).WithMany(x => x.Students).HasForeignKey(x => x.GenderId).HasConstraintName("FK_Students_Genders_GenderId");
            
            //------builder.HasQueryFilter(s => s.IsDeleted == false);//bu hemise basadusecek ki biz IsD i true olanlari getirmeliyem
        }
    }
}
