namespace TaskManagementApp.Services.AuthenticationApi.Services
{
    
    public class TokenProvider : ITokenProvider
    {
        /*
        public string CreateToken(AppUser user, IEnumerable<Role> roles)
        {

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
        */
    }

}
