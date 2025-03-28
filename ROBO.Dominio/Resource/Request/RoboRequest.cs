using ROBO.Dominio.Enums;
using ROBO.Dominio.Resource.Base;

namespace ROBO.Dominio.Resource.Request
{
    public class RoboRequest : ResourceBase
    {
        public BracoDireitoCotovelo BracoDireitoCotovelo { get; set; }
        public BracoDireitoPulso BracoDireitoPulso { get; set; }
        public BracoEsquerdoCotovelo BracoEsquerdoCotovelo { get; set; }
        public BracoEsquerdoPulso BracoEsquerdoPulso { get; set; }
        public CabecaInclinacao CabecaInclinacao { get; set; }
        public CabecaRotacao CabecaRotacao { get; set; }
    }
}
