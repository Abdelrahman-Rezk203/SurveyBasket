using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Extentions;
using SurveyBasket.API.Models;
using SurveyBasket.API.Persistance.ConfigurationFluentAPI;
using System.Reflection;
using System.Security.Claims;

namespace SurveyBasket.API.Persistance.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IHttpContextAccessor httpContextAccessor):base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Question> Questions { get; set; } 
        public DbSet<Vote> Votes  { get; set; } 
        public DbSet<VoteAnswer> VoteAnswers { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new PollConfiguration()); //Fluent apiهيه اللي هتشغل اال
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//IEntityTypeCOnfiguration هينفذ اي كلاس بيعمل امبليمنت ل 
           
            var CascadeFk = modelBuilder.Model.GetEntityTypes()      //loop on all entity
                                              .SelectMany(t => t.GetForeignKeys()) //select all fk for this entity
                                              .Where(f => f.DeleteBehavior == DeleteBehavior.Cascade && !f.IsOwnership);//choose Cascade only
            foreach (var fk in CascadeFk)
                fk.DeleteBehavior = DeleteBehavior.Restrict;        //change all Fk Cascade to Restrict
            
            base.OnModelCreating(modelBuilder);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries<AuditLoggingEntitiy>();//all classes inherit from AuditLoggingEntitiy
          
            foreach (var entity in entities)
            {
               var UserId = _httpContextAccessor.HttpContext?.User.GetUserId()!; //get UserId that Added
                if (entity.State == EntityState.Added)
                {
                    entity.Property(x => x.CreatedById).CurrentValue = UserId;
                }
                else if (entity.State == EntityState.Modified)
                {
                    entity.Property(x => x.UpdatedById).CurrentValue = UserId;
                    entity.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                }

            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
} 
