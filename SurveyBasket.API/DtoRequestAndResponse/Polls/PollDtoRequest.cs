using Microsoft.AspNetCore.Mvc.ModelBinding;
using SurveyBasket.API.Models;
using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.Dto.Polls
{
    public record PollDtoRequest(
        //[Required]
        string Title,
        [Required]
        string Description,
        DateOnly EndsAt,
        DateOnly StartsAt
        );
    //{
    //    public string Title { get; set; }
    //    public string Description { get; set; }

    //    //public static implicit operator Poll(PollDto pollDto)
    //    //{
    //    //    return new()
    //    //    {
    //    //        Description = pollDto.Description,
    //    //        Title = pollDto.Title,
                
    //    //    };
    //    //}
    //}
}
