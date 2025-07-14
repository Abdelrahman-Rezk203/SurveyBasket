using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.API.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
//using option pattern for this file
namespace SurveyBasket.API.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IOptions<JwtOptionPattern> _jwtOption;

        public JwtProvider(IOptions<JwtOptionPattern> jwtOption)
        {
            _jwtOption = jwtOption;
        }
        public (string token, int expireIn) GenerateToken(ApplicationUser applicationUser,IConfiguration configuration,IEnumerable<string> roles , IEnumerable<string> Permissions)
        {
            Claim[] claims =
                [
                    new(JwtRegisteredClaimNames.Sub, applicationUser.Id),
                    new(JwtRegisteredClaimNames.Email,applicationUser.Email!), //email can't Empty
                    new(JwtRegisteredClaimNames.GivenName,applicationUser.FirstName),
                    new(JwtRegisteredClaimNames.FamilyName,applicationUser.LastName),
                    new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new(nameof(roles) , JsonSerializer.Serialize(roles) , JsonClaimValueTypes.JsonArray),
                    new(nameof(Permissions) , JsonSerializer.Serialize(Permissions) , JsonClaimValueTypes.JsonArray)

                ];

            var symmtricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.Key)); //if you want to use a different key, you can change it in the appsettings.json file

            var signingCredintial = new SigningCredentials(symmtricSecurityKey, SecurityAlgorithms.HmacSha256);

            int expireIn = _jwtOption.Value.ExpiryMinuties * 60;

            var token = new JwtSecurityToken
                (
                    issuer: _jwtOption.Value.Issuer,
                    audience: configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtOption.Value.ExpiryMinuties) ,
                    signingCredentials: signingCredintial

                );

            return (token: new JwtSecurityTokenHandler().WriteToken(token),expireIn: expireIn );
                             
        }

        public string? ValidateToken(string token)
        {
            var TokenHandler = new JwtSecurityTokenHandler(); // encoding or decoding for token 
            var symmtricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.Key)); 
            //key fot encode or decode
            try
            {


                TokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = symmtricSecurityKey, 
                    ValidateIssuerSigningKey = true,  
                    ValidateIssuer = false, 
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero 


                }, out SecurityToken validatedToken);

                var JwtToken = (JwtSecurityToken)validatedToken;

                return JwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            }
            catch
            {
                return null;
            }
        }
    } 
}
