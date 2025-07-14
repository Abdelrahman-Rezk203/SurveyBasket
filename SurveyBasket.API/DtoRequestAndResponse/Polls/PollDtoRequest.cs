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
  
}
