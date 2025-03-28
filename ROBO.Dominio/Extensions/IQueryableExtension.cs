using System.Linq.Expressions;
using System.Reflection;

namespace ROBO.Dominio.Extensions
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> dados, int pagina, int quantidadeRegistros)
        {
            if (pagina == 0 || quantidadeRegistros == 0)
            {
                return dados;
            }
            else
            {
                return dados.Skip((pagina - 1) * quantidadeRegistros)
                    .Take(quantidadeRegistros);
            }
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string field)
        {
            return ApplyOrdering(source, field, "OrderBy");
        }

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string field)
        {
            return ApplyOrdering(source, field, "OrderByDescending");
        }

        private static IQueryable<T> ApplyOrdering<T>(IQueryable<T> source, string field, string methodName)
        {
            if (string.IsNullOrEmpty(field))
                throw new ArgumentException("O campo não pode ser nulo ou vazio.", nameof(field));

            // Obtém o tipo da entidade (T)
            var type = typeof(T);

            // Converte o nome do campo para o formato de propriedade de navegação
            // Ex: "entidadeDescricao" -> "Entidade.Descricao"
            var navigationPath = ResolveNavigationPath(type, field);
            if (string.IsNullOrEmpty(navigationPath))
                throw new ArgumentException($"O campo '{field}' não pôde ser resolvido para uma propriedade de navegação válida.", nameof(field));

            // Divide o campo em partes para lidar com propriedades de navegação
            var properties = navigationPath.Split('.');
            var parameter = Expression.Parameter(type, "x");

            Expression propertyAccess = parameter;
            foreach (var prop in properties)
            {
                var property = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                    throw new ArgumentException($"A propriedade '{prop}' não foi encontrada no tipo '{type.Name}'.", nameof(field));

                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                type = property.PropertyType;
            }

            // Cria uma expressão do tipo x => x.Property
            var lambda = Expression.Lambda(propertyAccess, parameter);

            // Cria uma chamada ao método OrderBy/OrderByDescending
            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(T), type },
                source.Expression,
                Expression.Quote(lambda)
            );

            // Retorna o IQueryable ordenado
            return source.Provider.CreateQuery<T>(resultExpression);
        }

        private static string ResolveNavigationPath(Type type, string field)
        {
            // Verifica se o campo é uma propriedade direta
            var directProperty = type.GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (directProperty != null)
                return field; // Retorna o campo diretamente

            // Tenta encontrar uma propriedade de navegação com base no nome do campo
            foreach (var property in type.GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance))
            {
                // Verifica se o nome do campo começa com o nome da propriedade
                if (field.StartsWith(property.Name, StringComparison.OrdinalIgnoreCase))
                {
                    // Remove o nome da propriedade do campo e tenta resolver o restante
                    var remainingField = field.Substring(property.Name.Length);
                    if (string.IsNullOrEmpty(remainingField))
                        continue;

                    // Verifica se a propriedade é uma classe (navegação)
                    if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    {
                        // Resolve o restante do caminho recursivamente
                        var nestedPath = ResolveNavigationPath(property.PropertyType, remainingField);
                        if (!string.IsNullOrEmpty(nestedPath))
                            return $"{property.Name}.{nestedPath}";
                    }
                }
            }

            return null; // Nenhum caminho de navegação encontrado
        }
    }
}
