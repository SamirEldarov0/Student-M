using Lesson30_WebApi.Configurations;
using Lesson30_WebApi.UnitOfWork;
using Lesson30_WebApi.Data.DAL;
using Lesson30_WebApi.Repository;
using Lesson30_WebApi.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Lesson30_WebApi.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Lesson30_WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace Lesson30_WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(x =>
               x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve).AddFluentValidation();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer=false,//authda tokeni yaradanda tokeni yaradan hissesinde vermemisem bunu kim issuer edip,generate edip ona gore 401 qayidir
                    ValidateAudience=false,
                    ValidateLifetime=true,
                    ValidateIssuerSigningKey=true,
                    ValidIssuer = "example.com",
                    ValidAudience = "example.com",
                    IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes("key"))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role,"Admin"));//RequireUserName adamin unami olsun(usernami flay sey oln adam olsun)
                options.AddPolicy("SuperUser", policy => policy.RequireClaim(ClaimTypes.Role, "Admin").RequireClaim(ClaimTypes.Role,"User"));
                options.AddPolicy("AdminRole", policy => policy.RequireRole("AdminRole"));
                options.AddPolicy("SamirOnly", policy => policy.RequireUserName("samir.eldarov"));//login olanin username-i budusa
                options.AddPolicy("OneOfTheRoles", policy => policy.RequireAssertion(context => context.User.HasClaim(claim => claim.Type == ClaimTypes.Role && claim.Value
                is "Admin" or "User")));
            });

            services.AddTransient<IValidator<StudentModel>, StudentValidator>();
            services.AddDbContext<StudentDbContext>(options =>//dependency injectiona scoped olaraq elave edir
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));//Mysqlde UseMySqlServer olur
            });
            
            //opt =>
            //{
            //    opt.Filters.Clear();
            //    opt.Filters.Add(new ProducesAttribute("application/xml"));
            //}).AddXmlSerializerFormatters();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lesson30_WebApi", Version = "v1" });
            });
            services.AddSingleton<IUser, User>();
            services.AddSingleton<ISingletonOperation, Operation>();
            services.AddScoped<IScopedOperation, Operation>();
            services.AddTransient<ITransientOperation, Operation>();
            services.Configure<PositionOptions>(Configuration.GetSection(PositionOptions.Position));//getir map et PositionOptions
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient(typeof(IGenericRepository<,>), typeof(EFGenericRepository<,>));
            services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("Hello world!");
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();//ekrana error cixanda 
                app.UseSwagger();//discover et
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lesson30_WebApi v1"));//ui-da goster
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //app.UseMiddleware<JwtMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
