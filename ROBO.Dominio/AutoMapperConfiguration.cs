using AutoMapper;
using EspecificacaoAnalise.Dominio.Entidades;
using EspecificacaoAnalise.Dominio.Resource.Request;
using EspecificacaoAnalise.Dominio.Resource.Response;

namespace ROBO.Dominio
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            var entityAssemplyEntity = typeof(Entidade).Assembly;
            var entityAssemplyRequest = typeof(EntidadeRequest).Assembly.ExportedTypes.ToList();
            var entityAssemplyResponse = typeof(EntidadeResponse).Assembly.ExportedTypes.ToList();

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

                    CreateMap<Entidade, EntidadeResponse>()
                    .ForMember(dest => dest.ProjetoDescricao, opt =>
                    {
                        opt.PreCondition(src => src.Projeto != null);
                        opt.MapFrom(src => src.Projeto.Descricao);
                    });

                    CreateMap<EntidadeAcao, EntidadeAcaoResponse>()
                    .ForMember(dest => dest.EntidadeDescricao, opt =>
                    {
                        opt.PreCondition(src => src.Entidade != null);
                        opt.MapFrom(src => src.Entidade.Descricao);
                    })
                    .ForMember(dest => dest.AcaoDescricao, opt =>
                    {
                        opt.PreCondition(src => src.Acao != null);
                        opt.MapFrom(src => src.Acao.Descricao);
                    });
                }
            });

        }
    }
}
