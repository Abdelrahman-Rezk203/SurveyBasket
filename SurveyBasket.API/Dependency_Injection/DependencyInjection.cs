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
                .AddScoped<IPollService, PollService>()
                .AddScoped<IAuth, AuthService>()
                .AddScoped<IQuestionService, QuestionService>()
                .AddScoped<IVoteSevice, VoteService>()
                .AddScoped<IResultVote, ResultVoteService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<ICacheService, CacheService>()
                .AddScoped<IUserService, UserService>()
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
            service
                .AddFluentValidationAutoValidation() 
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
            //service.Configure<JwtOptionPattern>(configuration.GetSection(JwtOptionPattern.SectionName)); 
            
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


            service.AddAuthentication(option =>  
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
            {
                o.SaveToken = true; 
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
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
                options.SignIn.RequireConfirmedEmail = true;   
                options.User.RequireUniqueEmail = true;
            });

            service.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

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
