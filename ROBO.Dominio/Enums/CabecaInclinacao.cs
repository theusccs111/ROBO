using System.ComponentModel;

namespace ROBO.Dominio.Enums
{
    public enum CabecaInclinacao
    {
        [Description("Para cima")]
        ParaCima = 0,
        [Description("Em Repouso")]
        EmRepouso = 1,
        [Description("Para Baixo")]
        ParaBaixo = 2
    }
}
