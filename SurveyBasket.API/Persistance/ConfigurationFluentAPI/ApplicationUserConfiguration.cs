using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Entities;


namespace SurveyBasket.API.Persistance.ConfigurationFluentAPI
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                    .OwnsMany(x => x.RefreshTokens)
                    .ToTable("RefreshTokens") //new name of table in database
                    .WithOwner() 
                    .HasForeignKey("UserId"); 

            builder.Property(x => x.FirstName).HasMaxLength(50);

            builder.Property(x => x.LastName).HasMaxLength(50);

            //var PasswordHasher = new PasswordHasher<ApplicationUser>();

            builder.HasData(new ApplicationUser
            {
                Id = DefaultUsers.AdminId,
                Email = DefaultUsers.AdminEmail,
                FirstName = DefaultUsers.AdminFirstName,
                LastName = DefaultUsers.AdminLastName,
                ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
                SecurityStamp = DefaultUsers.AdminSecurityStamp,
                NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
                NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
                UserName = DefaultUsers.AdminEmail,
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEHjXPM/W5gcdJHv3yTU9N1jFfHSwZD/UUl0NFvyb0YI8k5XxK+bvgZs6oaSztFvwjQ==" 
            });

        }
    }
}
