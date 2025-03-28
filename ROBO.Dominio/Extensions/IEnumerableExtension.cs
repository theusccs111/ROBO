using FluentValidation.Results;
using ROBO.Dominio.Exceptions;

namespace ROBO.Dominio.Extensions
{
    public static class IEnumerableExtension
    {
        public static void ThrowException(this IEnumerable<string> errors)
        {
            if (errors.Any())
            {
                List<ValidationFailure> failures = new List<ValidationFailure>();
                foreach (var error in errors)
                {
                    failures.Add(new ValidationFailure("Excecao", error));
                }

                throw new ValidationException(failures);
            }
        }

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
    }
}
