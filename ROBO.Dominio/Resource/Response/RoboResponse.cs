using ROBO.Dominio.Enums;
using ROBO.Dominio.Extensions;
using ROBO.Dominio.Resource.Base;

namespace ROBO.Dominio.Resource.Response
{
    public class RoboResponse : ResourceBase
    {
        public BracoDireitoCotovelo BracoDireitoCotovelo { get; set; }
        public string BracoDireitoCotoveloDescricao { get { return BracoDireitoCotovelo.GetDescription(); } }
        public BracoDireitoPulso BracoDireitoPulso { get; set; }
        public string BracoDireitoPulsoDescricao { get { return BracoDireitoPulso.GetDescription(); } }
        public BracoEsquerdoCotovelo BracoEsquerdoCotovelo { get; set; }
        public string BracoEsquerdoCotoveloDescricao { get { return BracoEsquerdoCotovelo.GetDescription(); } }
        public BracoEsquerdoPulso BracoEsquerdoPulso { get; set; }
        public string BracoEsquerdoPulsoDescricao { get { return BracoEsquerdoPulso.GetDescription(); } }
        public CabecaInclinacao CabecaInclinacao { get; set; }
        public string CabecaInclinacaoDescricao { get { return CabecaInclinacao.GetDescription(); } }
        public CabecaRotacao CabecaRotacao { get; set; }
        public string CabecaRotacaoDescricao { get { return CabecaRotacao.GetDescription(); } }
    }
}
