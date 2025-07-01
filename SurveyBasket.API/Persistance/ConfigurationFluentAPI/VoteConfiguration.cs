using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Persistance.ConfigurationFluentAPI
{
    public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {

            builder.HasIndex(x => new { x.PollId, x.UserId }).IsUnique(); // مينفعش اليوزر يصوت علي نفس البول تاني هيه مره واحده 
            


        }
    }
}
