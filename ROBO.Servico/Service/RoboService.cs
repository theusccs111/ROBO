using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ROBO.Dominio.Entidades;
using ROBO.Dominio.Resource.Base;
using ROBO.Dominio.Resource.Request;
using ROBO.Dominio.Resource.Response;
using ROBO.Servico.Interfaces;
using ROBO.Servico.Service;
using System.Drawing;

namespace EspecificacaoAnalise.Servico.Service
{
    public class RoboService : Service<Robo, RoboRequest>
    {
        public RoboService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IUnityOfWork uow, IConfiguration config, IValidator<Robo> validator) : base(httpContextAccessor, mapper, uow, config, validator)
        {
        }

        public override Resultado<RoboRequest> Add(RoboRequest request)
        {
            var result = base.Add(request);
            base.Complete();

            return result;
        }

        public override Resultado<RoboRequest[]> AddMany(RoboRequest[] request)
        {
            var result = base.AddMany(request);
            base.Complete();

            return result;
        }

        public override Resultado<RoboRequest> Update(RoboRequest request)
        {
            var result = base.Update(request);
            base.Complete();

            return result;
        }

        public override Resultado<RoboRequest[]> UpdateMany(RoboRequest[] request)
        {
            var result = base.UpdateMany(request);
            base.Complete();

            return result;
        }

        public override Resultado<RoboRequest> Delete(RoboRequest request)
        {
            var result = base.Delete(request);
            base.Complete();

            return result;
        }
        public override Resultado<RoboRequest[]> DeleteMany(RoboRequest[] request)
        {
            var result = base.DeleteMany(request);
            base.Complete();

            return result;
        }

        
    }
}
