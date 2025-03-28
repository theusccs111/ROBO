using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ROBO.Dominio.Resource.Response;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ROBO.Servico.Helpers
{
    public static class AuthResponseHelper
    {
        public static AuthResponse ObterLoginResponse(IHttpContextAccessor _httpContextAccessor)
        {
            AuthResponse authResponse = new AuthResponse();
            if (_httpContextAccessor.HttpContext != null)
            {
                var roboClaims = ClaimsHelper.ObterInformacaoDoClaims(_httpContextAccessor.HttpContext, "Robo");
                authResponse.Robo = roboClaims != null ? JsonConvert.DeserializeObject<RoboResponse>(roboClaims) : new RoboResponse();

            }
            return authResponse;
        }

        public static string AlterarIdentidade(AuthResponse loginResponse, IHttpContextAccessor _httpContextAccessor, IConfiguration Config)
        {
            var identity = _httpContextAccessor.HttpContext.User.Identities.First();

            identity.TryRemoveClaim(identity.FindFirst("Robo"));
            identity.AddClaim(new Claim("Robo", JsonConvert.SerializeObject(loginResponse.Robo)));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Config["JwtSecurityToken:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenReturn = tokenHandler.WriteToken(token);
            return tokenReturn;
        }
    }
}
