using Microsoft.AspNetCore.Mvc.ModelBinding;
using SurveyBasket.API.Entities;
using SurveyBasket.API.Models;
using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.Dto.Polls
{
    public record PollResponse(
        int Id ,
        string Title,
        string Description,
        bool IsPublisher,
        DateOnly EndsAt, 
        DateOnly StartsAt
        );
    
}
