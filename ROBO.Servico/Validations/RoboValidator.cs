using FluentValidation;
using FluentValidation.Results;
using ROBO.Dominio.Entidades;
using ROBO.Dominio.Enums;

namespace ROBO.Servico.Validations
{
    public class RoboValidator : AbstractValidator<Robo>
    {
        public RoboValidator()
        {
        }

        public ValidationResult Validate(Robo novoEstado, Robo estadoAtual)
        {
            RuleFor(robo => robo.BracoDireitoPulso)
                .NotNull()
                .Must((robo, pulso) => PodeMoverPulsoDireito(estadoAtual.BracoDireitoCotovelo, novoEstado.BracoDireitoPulso))
                .WithMessage("O Pulso direito só pode se movimentar se o Cotovelo direito estiver Fortemente Contraído.");

            RuleFor(robo => robo.BracoDireitoCotovelo)
            .NotNull()
            .Must(_ => ProgressoValido(estadoAtual.BracoDireitoCotovelo, novoEstado.BracoDireitoCotovelo))
            .WithMessage("A progressão do Cotovelo direito deve seguir a ordem correta.");

            RuleFor(robo => robo.BracoDireitoPulso)
                .NotNull()
                .Must(_ => ProgressoValido(estadoAtual.BracoDireitoPulso, novoEstado.BracoDireitoPulso))
                .WithMessage("A progressão do Pulso direito deve seguir a ordem correta.");

            RuleFor(robo => robo.BracoEsquerdoPulso)
                .NotNull()
                .Must((robo, pulso) => PodeMoverPulsoEsquerdo(estadoAtual.BracoEsquerdoCotovelo, novoEstado.BracoEsquerdoPulso))
                .WithMessage("O Pulso esquerdo só pode se movimentar se o Cotovelo direito estiver Fortemente Contraído.");


            RuleFor(robo => robo.BracoEsquerdoCotovelo)
            .NotNull()
            .Must(_ => ProgressoValido(estadoAtual.BracoEsquerdoCotovelo, novoEstado.BracoEsquerdoCotovelo))
            .WithMessage("A progressão do Cotovelo esquerdo deve seguir a ordem correta.");

            RuleFor(robo => robo.BracoEsquerdoPulso)
                .NotNull()
                .Must(_ => ProgressoValido(estadoAtual.BracoEsquerdoPulso, novoEstado.BracoEsquerdoPulso))
                .WithMessage("A progressão do Pulso esquerdo deve seguir a ordem correta.");

            RuleFor(robo => robo.CabecaRotacao)
          .NotNull()
          .Must(_ => PodeRotacionarCabeca(estadoAtual.CabecaInclinacao, novoEstado.CabecaRotacao))
          .WithMessage("A cabeça não pode ser rotacionada se estiver inclinada para baixo.");

            return base.Validate(novoEstado);
        }

        private bool PodeMoverPulsoDireito(BracoDireitoCotovelo cotovelo, BracoDireitoPulso pulso)
        {
            return cotovelo == BracoDireitoCotovelo.FortementeContraido || pulso == BracoDireitoPulso.EmRepouso;
        }

        private bool PodeMoverPulsoEsquerdo(BracoEsquerdoCotovelo cotovelo, BracoEsquerdoPulso pulso)
        {
            return cotovelo == BracoEsquerdoCotovelo.FortementeContraido || pulso == BracoEsquerdoPulso.EmRepouso;
        }

        private bool ProgressoValido<TEnum>(TEnum estadoAtual, TEnum novoEstado) where TEnum : Enum
        {
            int atual = Convert.ToInt32(estadoAtual);
            int novo = Convert.ToInt32(novoEstado);
            return novo == atual || novo == atual + 1 || novo == atual - 1;
        }

        private bool PodeRotacionarCabeca(CabecaInclinacao inclinacao, CabecaRotacao rotacao)
        {
            return inclinacao != CabecaInclinacao.ParaBaixo || rotacao == CabecaRotacao.EmRepouso;
        }
    }

}
