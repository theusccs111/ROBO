using System.ComponentModel;

namespace ROBO.Dominio.Enums
{
    public enum BracoDireitoCotovelo
    {
        [Description("Em Repouso")]
        EmRepouso = 0,
        [Description("Levemente Contraído")]
        LevementeContraido = 1,
        [Description("Contraído")]
        Contraido = 2,
        [Description("Fortemente Contraído")]
        FortementeContraido = 3,
    }
}
