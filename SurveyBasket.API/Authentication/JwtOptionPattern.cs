using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.Authentication
{
    public class JwtOptionPattern
    {
        public static string SectionName = "Jwt";
        [Required]
        public string Key { get; init; } = string.Empty;
        [Required]
        public string Issuer { get; init; } = string.Empty;
        [Required]
        public string Audience { get; init; } = string.Empty;
        [Required]
        [Range(1,int.MaxValue ,ErrorMessage = "Invalid Expiry Time")] // if period is less than 1 minute, it will throw an error
        public int ExpiryMinuties { get; init; } 

    } 
} 
 