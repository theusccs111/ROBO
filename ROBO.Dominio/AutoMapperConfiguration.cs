using AutoMapper;
using ROBO.Dominio.Entidades;
using ROBO.Dominio.Resource.Request;
using ROBO.Dominio.Resource.Response;

namespace ROBO.Dominio
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            var entityAssemplyEntity = typeof(Robo).Assembly;
            var entityAssemplyRequest = typeof(RoboRequest).Assembly.ExportedTypes.ToList();
            var entityAssemplyResponse = typeof(RoboResponse).Assembly.ExportedTypes.ToList();

            entityAssemplyEntity.ExportedTypes.ToList().ForEach(s =>
            {
                var formattedRequestModelName = string.Format("{0}Request", s.Name);
                var requestModelName = entityAssemplyRequest.FirstOrDefault(s => s.Name == formattedRequestModelName);
                if (requestModelName != null)
                {
                    this.CreateMap(s, requestModelName).ReverseMap();
                }

                var formattedResponseName = string.Format("{0}Response", s.Name);
                var responseModelName = entityAssemplyResponse.FirstOrDefault(s => s.Name == formattedResponseName);
                if (responseModelName != null)
                {
                    var map = this.CreateMap(s, responseModelName).ReverseMap();
                }
            });

        }
    }
}
