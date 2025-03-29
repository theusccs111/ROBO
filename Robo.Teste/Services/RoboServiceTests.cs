using AutoMapper;
using EspecificacaoAnalise.Servico.Service;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using ROBO.Dominio.Entidades;
using ROBO.Dominio.Enums;
using ROBO.Dominio.Exceptions;
using ROBO.Dominio.Resource.Request;
using ROBO.Dominio.Resource.Response;
using ROBO.Servico.Helpers;
using ROBO.Servico.Service;
using ROBO.Servico.Validations;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace Robo.Teste.Services
{
    public class RoboServiceTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<RoboValidator> _validatorMock;
        private readonly RoboService _roboService;

        public RoboServiceTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapperMock = new Mock<IMapper>();
            _configMock = new Mock<IConfiguration>();
            _validatorMock = new Mock<RoboValidator>();

            _configMock.Setup(c => c["JwtSecurityToken:key"])
                     .Returns("uma-chave-super-secreta-longa-com-32-chars!!");

            _roboService = new RoboService(
                _httpContextAccessorMock.Object,
                _mapperMock.Object,
                _configMock.Object,
                _validatorMock.Object);
        }

        private AuthResponse CriarAuthResponsePadrao()
        {
            return new AuthResponse
            {
                Token = "token-teste",
                Robo = new RoboResponse
                {
                    BracoDireitoCotovelo = BracoDireitoCotovelo.EmRepouso,
                    BracoDireitoPulso = BracoDireitoPulso.EmRepouso,
                    BracoEsquerdoCotovelo = BracoEsquerdoCotovelo.EmRepouso,
                    BracoEsquerdoPulso = BracoEsquerdoPulso.EmRepouso,
                    CabecaInclinacao = CabecaInclinacao.EmRepouso,
                    CabecaRotacao = CabecaRotacao.EmRepouso
                }
            };
        }

        [Fact]
        public void Iniciar_DeveRetornarEstadoInicialValido()
        {
            // Act
            var result = _roboService.Iniciar();

            // Assert
            Assert.True(result.Success);
            Assert.Equal(BracoDireitoCotovelo.EmRepouso, result.Data.Robo.BracoDireitoCotovelo);
            Assert.Equal(BracoDireitoPulso.EmRepouso, result.Data.Robo.BracoDireitoPulso);
            Assert.Equal(BracoEsquerdoCotovelo.EmRepouso, result.Data.Robo.BracoEsquerdoCotovelo);
            Assert.Equal(BracoEsquerdoPulso.EmRepouso, result.Data.Robo.BracoEsquerdoPulso);
            Assert.Equal(CabecaInclinacao.EmRepouso, result.Data.Robo.CabecaInclinacao);
            Assert.Equal(CabecaRotacao.EmRepouso, result.Data.Robo.CabecaRotacao);
            Assert.NotNull(result.Data.Token);
        }

        [Fact]
        public void Mover_ComMovimentoValido_DeveAtualizarEstado()
        {
            // Arrange
            var authResponse = CriarAuthResponsePadrao();
            var request = new RoboRequest
            {
                BracoDireitoCotovelo = BracoDireitoCotovelo.LevementeContraido
            };

            var estadoAtual = new ROBO.Dominio.Entidades.Robo { BracoDireitoCotovelo = BracoDireitoCotovelo.EmRepouso };
            var novoEstado = new ROBO.Dominio.Entidades.Robo { BracoDireitoCotovelo = request.BracoDireitoCotovelo };

            SetupHttpContext(authResponse);

            _mapperMock.Setup(m => m.Map<ROBO.Dominio.Entidades.Robo>(authResponse.Robo))
                     .Returns(estadoAtual);

            _mapperMock.Setup(m => m.Map<ROBO.Dominio.Entidades.Robo>(request))
                     .Returns(novoEstado);

            _mapperMock.Setup(m => m.Map<RoboResponse>(It.IsAny<ROBO.Dominio.Entidades.Robo>()))
                     .Returns(new RoboResponse
                     {
                         BracoDireitoCotovelo = request.BracoDireitoCotovelo
                     });

            _validatorMock.Setup(v => v.Validate(It.IsAny<ROBO.Dominio.Entidades.Robo>(), It.IsAny<ROBO.Dominio.Entidades.Robo>()))
                .Returns(new ValidationResult());

            // Act
            var result = _roboService.Mover(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(BracoDireitoCotovelo.LevementeContraido, result.Data.Robo.BracoDireitoCotovelo);
        }



        private void SetupHttpContext(AuthResponse authResponse)
        {
            var claims = new[] { new Claim("Robo", JsonConvert.SerializeObject(authResponse.Robo)) };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };

            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContext);
        }
    }
}