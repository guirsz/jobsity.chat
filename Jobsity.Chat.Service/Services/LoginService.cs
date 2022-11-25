using Jobsity.Chat.Domain.Dtos.Login;
using Jobsity.Chat.Domain.Entities;
using Jobsity.Chat.Domain.Helpers;
using Jobsity.Chat.Domain.Interfaces.Repositories;
using Jobsity.Chat.Domain.Interfaces.Services;
using Jobsity.Chat.Domain.Security;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace Jobsity.Chat.Service.Services
{
    public class LoginService : ILoginService
    {
        private IUserRepository repository;
        private SigningConfigurations signingConfigurations;
        private TokenConfigurations tokenConfigurations;
        public IConfiguration configuration { get; }

        public LoginService(
                            IUserRepository repository,
                            SigningConfigurations signingConfigurations,
                            TokenConfigurations tokenConfigurations,
                            IConfiguration configuration)
        {
            this.repository = repository;
            this.signingConfigurations = signingConfigurations;
            this.tokenConfigurations = tokenConfigurations;
            this.configuration = configuration;
        }

        public async Task<object> Authenticate(LoginDto user)
        {
            var baseUser = new UserEntity();

            if (user != null && !string.IsNullOrWhiteSpace(user.Email) && !string.IsNullOrWhiteSpace(user.Password))
            {
                baseUser = await repository.FindByLogin(user.Email);

                if (baseUser != null && user.Password.VerifyHashedPassword(baseUser.Password))
                {
                    return SuccessObject(user.Email, baseUser.Id, baseUser.Name);
                }
            }
            return new
            {
                authenticated = false,
                message = "Authentication Failure."
            };
        }

        private object SuccessObject(string email, Guid userId, string name)
        {
            var identity = new ClaimsIdentity(
                new GenericIdentity(email),
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // jti token ID
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }
            );

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            string token = CreateToken(identity, createDate, expirationDate, handler);

            return new
            {
                authenticated = true,
                created = createDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                accessToken = token,
                userEmail = email,
                userName = name,
                message = "User logged in successfully."
            };
        }

        private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredencials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate                
            });

            var token = handler.WriteToken(securityToken);
            return token;
        }
    }
}
