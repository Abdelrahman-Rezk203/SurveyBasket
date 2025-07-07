using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.API.Authentication;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Persistance.DbContext;
using SurveyBasket.API.Repositories;
using SurveyBasket.API.Services;
using SurveyBasket.API.Settings;
using SurveyBasket.Authentication.Filters;
using System.Reflection;
using System.Text;
namespace SurveyBasket.API.Code_For_Program
{
    public static class DependencyInjection
    {
       
        public static IServiceCollection AddDependies(this IServiceCollection service,IConfiguration configuration)
        {

            service.AddControllers();

            service.AddCors(options =>
               options.AddDefaultPolicy(builder =>
                   builder
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!)
               )
           );

            service
                .AddMapster()
                .AddSwaggerServices()
                .AddFluentValidation()
                .AddDbcontext(configuration)
                .AddIdentityUserManager(configuration)
                .AddHybridCache();

            service.AddBackgroundJobConfig(configuration);

            service
                .AddScoped<IPollServices, PollServices>()
                .AddScoped<IAuth, AuthService>()
                .AddScoped<IQuestionService, QuestionService>()
                .AddScoped<IVoteSevice, VoteService>()
                .AddScoped<IResultVote, ResultVote>()
                .AddScoped<ICacheService, CacheService>()
                .AddScoped<IUserProfileService, UserProfileService>()
                .AddTransient<IEmailSender, EmailService>()
                ;
               
         
            return service;
        }

        public static IServiceCollection AddSwaggerServices(this IServiceCollection service)
        {
            service
               .AddEndpointsApiExplorer()
               .AddSwaggerGen();

            return service;
        }

        public static IServiceCollection AddMapster(this IServiceCollection service)
        {
           
            //جزء تشغيل الMapster
            var MappingConfig = TypeAdapterConfig.GlobalSettings;
            MappingConfig.Scan(Assembly.GetExecutingAssembly());
            service.AddSingleton<IMapper>(new Mapper(MappingConfig));

            return service;
        }

        public static IServiceCollection AddFluentValidation(this IServiceCollection service)
        {
            //كل مره لما اعمل فاليديشن علي كل كلاس AddScoped دا  بدل اني اعمل 
            service
                .AddFluentValidationAutoValidation() //دا عشان مكتبر هري كتير ف الكنترولر
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return service;
        }


        public static IServiceCollection AddDbcontext(this IServiceCollection service,IConfiguration configuration)
        {
            var connectionstring = configuration.GetConnectionString("DefaultConnection") ??
               throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

            service.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(connectionstring));
            return service;
        }

        public static IServiceCollection AddIdentityUserManager(this IServiceCollection service,IConfiguration configuration)
        {
            //service.Configure<JwtOptionPattern>(configuration.GetSection(JwtOptionPattern.SectionName)); //عشان يعرف اني شغال بال اوبشن باترن 
            
            service.AddOptions<JwtOptionPattern>()
                .BindConfiguration(JwtOptionPattern.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var JwtSettings = configuration.GetSection(JwtOptionPattern.SectionName).Get<JwtOptionPattern>();

            service.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            service.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

            service.AddSingleton<IJwtProvider, JwtProvider>();
            service.AddIdentity<ApplicationUser, ApplicationRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddDefaultTokenProviders();


            service.AddAuthentication(option =>  //عشان يبقي عارف نوع التوكن مش كل شويه اقوله انا بستخدم النووع ده bearer
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
            {
                o.SaveToken = true; //اقد راوصله طول منا شغال عادي 
                o.TokenValidationParameters = new TokenValidationParameters
                {//كلما زادت التعريفات دي زاد امان التوكن 
                    ValidateIssuer = true, //بيقارن القيم اللي جايه بالقيم اللي ف التوكن عشان يتاكد ان التوكن تممم 
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings?.Key!)),
                    ValidIssuer = JwtSettings?.Issuer,
                    ValidAudience = JwtSettings?.Audience
                };
            });

            service.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
                options.Password.RequiredLength = 8;
                  // لازم تعمل تاكيد للحساب عشان تقدر تدخل
                options.SignIn.RequireConfirmedEmail = true;   //عشان ننفذ دول  SignInManager  لازم استخدم انتفير ال  
                options.User.RequireUniqueEmail = true;
            });

            service.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));


            //var test = new  //عشان اتاكد هو قاري ولا لا  هحط debug
            //{
            //     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
            //        ValidIssuer = configuration["Jwt:Issuer"],
            //        ValidAudience = configuration["Jwt:Audience"]
            //};


            return service;
        }

        public static IServiceCollection AddBackgroundJobConfig(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"))); 
            
            return service;
        }
    }
}
