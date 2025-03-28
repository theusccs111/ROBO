using ROBO.Dominio.Entidades.Base;
using ROBO.Dominio.Enums;

namespace ROBO.Dominio.Entidades
{
    public class Robo : EntidadeBase
    {
        public BracoDireitoCotovelo BracoDireitoCotovelo { get; set; }
        public BracoDireitoPulso BracoDireitoPulso { get; set; }
        public BracoEsquerdoCotovelo BracoEsquerdoCotovelo { get; set; }
        public BracoEsquerdoPulso BracoEsquerdoPulso { get; set; }
        public CabecaInclinacao CabecaInclinacao { get; set; }
        public CabecaRotacao CabecaRotacao { get; set; }
    }
}
