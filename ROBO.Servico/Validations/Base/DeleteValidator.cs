using FluentValidation;
using ROBO.Dominio.Entidades.Base;
using ROBO.Servico.Interfaces.Repository;

namespace ROBO.Servico.Validations.Base
{
    public class DeleteValidator<T> : AbstractValidator<T> where T : EntidadeBase
    {
        public DeleteValidator(IRepository<T> repository)
        {
            RuleFor(x => x.Id)
                .Must(id => repository.Exists(id))
                .WithMessage("Registro com ID {PropertyValue} não encontrado.");
        }
    }
}
