using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.API.Entities
{
    public class ApplicationUser : IdentityUser 
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public bool IsDisabled { get; set; }
        public ICollection<Vote> Votes { get; set; } = [];
        public List<RefreshToken> RefreshTokens { get; set; }  = new List<RefreshToken>();
                                                                                    
    }
}

