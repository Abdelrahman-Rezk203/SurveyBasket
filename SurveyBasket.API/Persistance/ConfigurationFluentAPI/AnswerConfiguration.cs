using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Persistance.ConfigurationFluentAPI
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            //مينفعش يكون الكونتنت متكرر لنفس السوال
            //unique يعني السوال والجابه يكونوا 
            builder.HasIndex(x => new {x.QuestionId , x.Content}).IsUnique(); //مش هكرر نفس الاجابه مع نفس السوال 
            builder.Property(x => x.Content).HasMaxLength(1000);

          

        }
    }
}
