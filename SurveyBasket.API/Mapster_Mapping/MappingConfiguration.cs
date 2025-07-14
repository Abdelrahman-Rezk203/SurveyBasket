using Mapster;
using SurveyBasket.API.Dto.Polls;
using SurveyBasket.API.DtoRequestAndResponse.Question;
using SurveyBasket.API.DtoRequestAndResponse.Users;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Mapster_Mapping
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //var confisg = new TypeAdapterConfig();
            config.NewConfig<PollDtoRequest, Poll>().Map(x => x.Description, y => y.Description); 

            
            config.NewConfig<QuestionRequest, Question>()
                 .Map(x => x.Answers, x => x.Answers.Select(x => new Answer { Content = x })); 



            config.NewConfig<CreateUserRequest, ApplicationUser>()
                .Map(dest => dest.UserName, src => src.Email)
                .Map(dest => dest.EmailConfirmed, src => true);

        }
    }
}
