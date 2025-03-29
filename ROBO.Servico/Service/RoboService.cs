using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ROBO.Dominio.Entidades;
using ROBO.Dominio.Enums;
using ROBO.Dominio.Resource.Base;
using ROBO.Dominio.Resource.Request;
using ROBO.Dominio.Resource.Response;
using ROBO.Servico.Helpers;
using ROBO.Servico.Service;
using ROBO.Servico.Validations;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EspecificacaoAnalise.Servico.Service
{
    public class RoboService : Service<Robo, RoboRequest>
    {
        private readonly RoboValidator _validator;
        public RoboService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IConfiguration config, RoboValidator validator) : base(httpContextAccessor, mapper, config)
        {
            _validator = validator;
        }

        public Resultado<AuthResponse> Iniciar()
        {
            var token = SalvarRoboNoClaims();
            var response = new AuthResponse()
            {
                Token = token,
                Robo = new RoboResponse()
                {
                    BracoDireitoCotovelo = BracoDireitoCotovelo.EmRepouso,
                    BracoDireitoPulso = BracoDireitoPulso.EmRepouso,
                    BracoEsquerdoCotovelo = BracoEsquerdoCotovelo.EmRepouso,
                    BracoEsquerdoPulso = BracoEsquerdoPulso.EmRepouso,
                    CabecaInclinacao = CabecaInclinacao.EmRepouso,
                    CabecaRotacao = CabecaRotacao.EmRepouso,
                }
            };

            return new Resultado<AuthResponse>(response);
        }

        private string SalvarRoboNoClaims()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Config["JwtSecurityToken:key"]);

            RoboResponse robo = new RoboResponse()
            {
                BracoDireitoCotovelo = BracoDireitoCotovelo.EmRepouso,
                BracoDireitoPulso = BracoDireitoPulso.EmRepouso,
                BracoEsquerdoCotovelo = BracoEsquerdoCotovelo.EmRepouso,
                BracoEsquerdoPulso = BracoEsquerdoPulso.EmRepouso,
                CabecaInclinacao = CabecaInclinacao.EmRepouso,
                CabecaRotacao = CabecaRotacao.EmRepouso,
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Robo", JsonConvert.SerializeObject(robo)),
                }),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenReturn = tokenHandler.WriteToken(token);
            return tokenReturn;
        }

        public Resultado<AuthResponse> ObterRobo()
        {
            return new Resultado<AuthResponse>(Robo);
        }

        public Resultado<AuthResponse> Mover(RoboRequest request)
        {
            var estadoAtual = Mapper.Map<Robo>(Robo.Robo); 
            var novoEstado = Mapper.Map<Robo>(request);

            var validReturn = _validator.Validate(novoEstado, estadoAtual);

            if (!validReturn.IsValid)
                throw new ROBO.Dominio.Exceptions.ValidationException(validReturn.Errors.ToList());

            Robo.Robo = Mapper.Map<RoboResponse>(novoEstado);

            string token = AuthResponseHelper.AlterarIdentidade(Robo, HttpContextAccessor, Config);
            Robo.Token = token;

            return new Resultado<AuthResponse>(Robo);
        }

    }
}
