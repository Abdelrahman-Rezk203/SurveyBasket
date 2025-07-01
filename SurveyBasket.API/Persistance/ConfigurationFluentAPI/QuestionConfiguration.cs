using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Persistance.ConfigurationFluentAPI
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            
            builder.HasIndex(x => new {x.PollID , x.Content}).IsUnique(); //مش هكرر نفس السوال جوه نفس البول 
            builder.Property(x => x.Content).HasMaxLength(1000);

           

        }
    }
}
