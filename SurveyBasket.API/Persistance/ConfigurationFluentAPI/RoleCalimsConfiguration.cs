using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.API.Abstractions.Consts;

namespace SurveyBasket.API.Persistance.ConfigurationFluentAPI
{
    public class RoleCalimsConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        {
            var permissions = Permissions.GetAllPermissions();
            var adminClaims = new List<IdentityRoleClaim<string>>(); // هضيف فيها 


            for(int i = 0; i < permissions.Count; i++)
            {
                adminClaims.Add(new IdentityRoleClaim<string>
                {
                    Id = i + 1, // هديها ايدي عشان تبقي unique
                    ClaimType = Permissions.Type,
                    ClaimValue = permissions[i], // هضيف ال value بتاعتها
                    RoleId = DefaultRoles.AdminRoleId
                });
            }

            builder.HasData(adminClaims); // هضيفها ف الداتا بيز
        }
    }
}
