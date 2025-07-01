using SurveyBasket.API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyBasket.API.Entities
{
    public class Question : AuditLoggingEntitiy
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        [ForeignKey("PollID")]
        public int PollID { get; set; }

        public Poll Poll { get; set; } = default!;

        public ICollection<Answer> Answers { set; get; } = [];
        public ICollection<VoteAnswer> VoteAnswers { set; get; } = [];




    }
}
