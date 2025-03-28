using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ROBO.Dominio.Entidades.Base;
using ROBO.Dominio.Resource.Base;
using ROBO.Dominio.Resource.Response;
using ROBO.Servico.Helpers;

namespace ROBO.Servico.Service
{
    public abstract class Service<T, R> where T : EntidadeBase where R : ResourceBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected IHttpContextAccessor HttpContextAccessor { get { return _httpContextAccessor; } }
        private readonly IMapper _mapper;
        protected IMapper Mapper { get { return _mapper; } }
        private readonly IConfiguration _config;
        protected IConfiguration Config { get { return _config; } }

        private AuthResponse _robo;
        protected AuthResponse Robo { get { return _robo; } set { _robo = value; } }
        public Service(IHttpContextAccessor httpContextAccessor, IMapper mapper, IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _config = config;
            _robo = AuthResponseHelper.ObterLoginResponse(_httpContextAccessor);
        }

        
    }
}
