using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Persistance.ConfigurationFluentAPI
{
    public class PollConfiguration : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(x => x.Title).IsUnique();
            builder.Property(x => x.Title).HasMaxLength(100);

            builder.Property(x => x.Description).HasMaxLength(1500);

            builder.Property(x => x.StartsAt).IsRequired();

        }
    }
}
