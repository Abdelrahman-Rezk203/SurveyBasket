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
            config.NewConfig<PollDtoRequest, Poll>().Map(x => x.Description, y => y.Description); //لما احول بين الاتنين دول روح هات الديسكريبشن من كذا عشان الاول الاسمين كانوا مختلفين مش هيعرف يجيبهم لوحده 

            //عشان اقدر اجيب الداتا من الانسر 
            //لما اعمل اد تتضاف وترجع ف الريسبونس بردوا 
            config.NewConfig<QuestionRequest, Question>()
                 .Map(x => x.Answers, x => x.Answers.Select(x => new Answer { Content = x })); //الانسر هجيبها من الكونتنت اللي ف الانسر عشان كده انا عملت اوبجكت
                                                                                               //طالما م عارف يحول انا بقوله الانسر هتحولها كده 
                                                                                               //لان الايور اللي طلع انو م عارف يحول من سترنج الي انسر 



            config.NewConfig<CreateUserRequest, ApplicationUser>()
                .Map(dest => dest.UserName, src => src.Email)
                .Map(dest => dest.EmailConfirmed, src => true);

        }
    }
}
