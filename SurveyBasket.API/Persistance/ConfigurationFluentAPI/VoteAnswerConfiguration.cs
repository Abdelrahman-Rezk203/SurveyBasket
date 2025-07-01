using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Persistance.ConfigurationFluentAPI
{
    public class VoteAnswerConfiguration : IEntityTypeConfiguration<VoteAnswer>
    {
        public void Configure(EntityTypeBuilder<VoteAnswer> builder)
        {
            
            builder.HasIndex(x => new {x.QuestionId , x.VoteId}).IsUnique(); //مينفعش اكرر نفس الفوت علي نفس السوال 

           

        }
    }
}
