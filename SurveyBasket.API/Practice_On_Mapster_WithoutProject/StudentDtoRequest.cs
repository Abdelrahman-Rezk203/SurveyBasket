using Mapster;
using Microsoft.OpenApi.Attributes;
using SurveyBasket.API.CustomAttributies;
using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.Practice_On_Mapster_WithoutProject
{
    public class StudentDtoRequest //دا اللي رايح للموديل بالداتا اللي جايه من اليوزر بعدين يبعتله الرد
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        
        [MinAgeAttributies(18),System.ComponentModel.DataAnnotations.Display(Name = "Date Of Birth")]
        //[MinAgeAttributies(22,ErrorMessage = "Date Of Birth Invalid")]
        public DateTime? DateOFBirth { get; set; }

        //[AdaptIgnore] لو هلغي انو يعم تلقالي
        public DepartmentDtoRequest Department { get; set; } = default!; //هيعمله تلقاي by convention


    }

    public class DepartmentDtoRequest
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }



} 
