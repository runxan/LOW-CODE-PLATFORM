using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WebApiCodeGenLib.Model;

namespace WebApiCodeGenLib.Helpers
{
    public class JWTTokenHandler
    {
        public static string SecretKey = "Gentech_secretkey!54321";
        public static string Issuer = "gentech";
        public static string Audience = "global";
        public string CreateToken(string username, string password)
        {
            var login = new LoginModel();
            var token = "";
            login.Username = username;
            login.Password = password;
            var user = Authenticate(login);
            if (user != null)
            {
                token = BuildToken(user);
            }
            return token;
        }
        public UserModel Authenticate(LoginModel login)
        {
            UserModel user = null;

            if (login.Username == "ashish" && login.Password == "gentech")
            {
                user = new UserModel { Name = "ashish khawas", Email = "ashish@generaltechnology.com.np" };
            }
            return user;
        }

        
        public string BuildToken(UserModel user)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Birthdate, user.Birthdate.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: Issuer, audience: Audience, claims: claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public bool ValidateToken(string authToken)
        {
            var response = true;
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            }
            catch (Exception ex)
            {
                response = false;
            }
            return response;
        }
        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)) // The same key as the one that generate the token
            };
        }
    }
}
