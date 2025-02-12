
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace TaskManagementApp.Services.AuthenticationApi.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly SecretHandler _secretHandler;
        private readonly string _encryptedJwtPropertiesPath;

        public TokenProvider(SecretHandler secretHandler, string encryptedJwtPropertiesPath)
        {
            _secretHandler = secretHandler;
            _encryptedJwtPropertiesPath = encryptedJwtPropertiesPath;
        }

        public string CreateToken(AppUser user, IEnumerable<Role> roles)
        {
            string encryptedData = File.ReadAllText(_encryptedJwtPropertiesPath);
            string decryptedData = _secretHandler.Decrypt(encryptedData);

            var jwtProperties = JsonSerializer
                .Deserialize<JwtProperties>(
                    decryptedData,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            var secretKey = jwtProperties!.Key;
            var key = Encoding.ASCII.GetBytes(secretKey!);
            var securityKey = new SymmetricSecurityKey(key);

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        };
            claims.AddRange(roles.Select(role
                => new Claim(ClaimTypes.Role, role.ToString())));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes((double)jwtProperties.Expiration!),
                SigningCredentials = credentials,
                Issuer = jwtProperties.Issuer,
                Audience = jwtProperties.Audience
            };

            var handler = new JsonWebTokenHandler();
            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}
