using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.API.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        public (string token, int expireIn) GenerateToken(ApplicationUser applicationUser,IConfiguration configuration)
        {
            Claim[] claims =
                [
                    new(JwtRegisteredClaimNames.Sub, applicationUser.Id),
                    new(JwtRegisteredClaimNames.Email,applicationUser.Email!), //email can't Empty
                    new(JwtRegisteredClaimNames.GivenName,applicationUser.FirstName),
                    new(JwtRegisteredClaimNames.FamilyName,applicationUser.LastName),
                    new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

                ];

            var symmtricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.Key)); //اللي هشفل بيه التوكن 

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
                             //JwtSecurityTokenHandler => مسؤول عن كريتت التوكن وفك التوكن 
        }

        public string? ValidateToken(string token)
        {
            var TokenHandler = new JwtSecurityTokenHandler(); // encoding or decoding for token 
            var symmtricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.Key)); //اللي هشفل بيه التوكن 
            //key fot encode or decode
            try
            {


                TokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = symmtricSecurityKey, //هقارن التوكن بعد فك التشفير باللي قبل الشتفير
                    ValidateIssuerSigningKey = true, //key لازم يكون معمول بنفس ال 
                    ValidateIssuer = false, 
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero // يعني لو التوكن منتهي حتى بثانية  يعتبر غير صالح .


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
