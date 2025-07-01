
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using SurveyBasket.API.Code_For_Program;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Models;
using SurveyBasket.API.Persistance.DbContext;
using SurveyBasket.API.Repositories;
using SurveyBasket.API.Services;
using System.Reflection;
using System.Text;

namespace SurveyBasket.API

{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // Add services to the container.

            /* builder.Services.AddControllers();
             // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

             builder.Services
                 .AddEndpointsApiExplorer()
                 .AddSwaggerGen();

             builder.Services
                 .AddScoped<IPollServices, PollServices>()
                 .AddScoped<List<Poll>>()
                 ;

             //كل مره لما اعمل فاليديشن علي كل كلاس AddScoped دا  بدل اني اعمل 
             builder.Services
                 .AddFluentValidationAutoValidation() //دا عشان مكتبر هري كتير ف الكنترولر
                 .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

             //جزء تشغيل الMapster
             var MappingConfig = TypeAdapterConfig.GlobalSettings;
             MappingConfig.Scan(Assembly.GetExecutingAssembly());

             builder.Services.AddSingleton<IMapper>(new Mapper(MappingConfig));
             ///////

            */

            //builder.Services.AddDependies();
            //builder.Services.AddSwaggerServices(); ممكن انادي كل واحده وحدها
            //builder.Services.AddMapster();

            //builder.Services.AddIdentityApiEndpoints<ApplicationUser>() //حاجات الاوثوريزيشن اتوماتيك 
            //       .AddEntityFrameworkStores<ApplicationDbContext>();
            //var configuration = builder.Configuration["MyKey"];

            builder.Services.AddDependies(builder.Configuration); //هنا  بناديهم جوه دي بعدين انادي واحده بس والباقي جواها مرتبط بيها ف الكود 
            builder.Services.AddDistributedMemoryCache();


            //Nswag Service
            builder.Services.AddOpenApiDocument(option =>
            {
                option.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Name = "Authorization",
                    Description = "Enter 'Bearer' followed by a space and the JWT token."
                });

                option.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
            });

            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });

           
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUI();
            }
            app.UseSerilogRequestLogging(); //[09:17:47 INF] HTTP POST /Auth/Login responded 400 in 1209.9089 ms
            app.UseHttpsRedirection();
           
            app.UseHangfireDashboard("/jobs");
            
            app.UseCors("MyPolicy");
            app.UseAuthorization();



            //app.MapIdentityApi<ApplicationUser>();

            app.MapControllers();

            app.Run();
        }
    }
}
