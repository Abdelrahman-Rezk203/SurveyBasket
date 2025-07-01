using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.API.Entities;

namespace SurveyBasket.API.Persistance.ConfigurationFluentAPI
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser> 
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                    .OwnsMany(x => x.RefreshTokens) //اسمها كذا Many عنده علاقه 
                    .ToTable("RefreshTokens") //اسم table الجديد
                    .WithOwner() //applicationUser وهو  many الواحد عنده  
                    .HasForeignKey("UserId"); //اسم الكلوم

            //لكن لو اليوزر بعت داتا اكتر من 50 هيقبلها بس لما يحطها ف الداتا بيز هيدي ايرور لكن هيقبل الريكويست عادي 
            builder.Property(x => x.FirstName).HasMaxLength(50); //بتكون علي الداتا بيز مينفعش تخزن اكتر من 50 حرف
              
            builder.Property(x => x.LastName).HasMaxLength(50);
        }
    }
}
