using SurveyBasket.API.Dto.Polls;
using SurveyBasket.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.Models
{
    public sealed class Poll : AuditLoggingEntitiy
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsPublisher { get; set; }
        public DateOnly StartsAt { get; set; }
        public DateOnly EndsAt { get; set; }

        public ICollection<Question> Questions { get; set; } = [];
        public ICollection<Vote> votes { get; set; } = [];

    }
}
