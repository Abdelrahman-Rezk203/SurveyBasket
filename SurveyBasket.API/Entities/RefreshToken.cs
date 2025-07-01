using Microsoft.EntityFrameworkCore;

namespace SurveyBasket.API.Entities
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;

        public DateTime ExpireIn { get; set; }

        public DateTime CreateOn { get; set; } = DateTime.UtcNow;

        public DateTime? RevokedOn { get; set; }

        public bool IsExpired =>  DateTime.UtcNow >= ExpireIn  ;

        public bool IsActivate => RevokedOn is null && !IsExpired;


    }
}
