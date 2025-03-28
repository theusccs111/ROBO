using System.ComponentModel;

namespace ROBO.Dominio.Enums
{
    public enum CabecaRotacao
    {
        [Description("Rotação para -90º")]
        RotacaoMenos90 = 0,
        [Description("Rotação para -45")]
        RotacaoMenos45 = 1,
        [Description("Em Repouso")]
        EmRepouso = 2,
        [Description("Rotação para 45º")]
        Rotacao45 = 3,
        [Description("Rotação para 90º")]
        Rotacao90 = 4
    }
}
