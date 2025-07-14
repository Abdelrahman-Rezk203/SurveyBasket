using SurveyBasket.API.Models;

namespace SurveyBasket.API.Entities
{
    public sealed class Vote
    {
        public int Id { get; set; }
        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;
        public int PollId { get; set; }
        public string UserId { get; set; } = string.Empty; //FK Auto By Ef core 
        public Poll Poll { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!; 
        public ICollection<VoteAnswer> VoteAnswers { get; set; } = [];

    }
}
