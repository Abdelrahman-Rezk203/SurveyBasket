using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Entities;

namespace SurveyBasket.API.Persistance.ConfigurationFluentAPI
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData([
                     new ApplicationRole
                     {
                         Id = DefaultRoles.AdminRoleId,
                         Name = DefaultRoles.Admin,
                         NormalizedName = DefaultRoles.Admin.ToUpper(),
                         ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp

                     },
                    new ApplicationRole
                    {
                        Id = DefaultRoles.MemberRoleId,
                        Name = DefaultRoles.Member,
                        NormalizedName = DefaultRoles.Member.ToUpper(),
                        ConcurrencyStamp = DefaultRoles.MemberRoleConcurrencyStamp,
                        IsDefault = true
                    }
                ]);
        }
    }
}
