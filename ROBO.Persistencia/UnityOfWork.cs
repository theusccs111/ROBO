using EspecificacaoAnalise.Persistencia.Data;
using ROBO.Dominio.Entidades;
using ROBO.Dominio.Entidades.Base;
using ROBO.Persistencia.Repositories;
using ROBO.Servico.Interfaces;
using ROBO.Servico.Interfaces.Repository;

namespace ROBO.Persistencia
{
    public class UnityOfWork : IUnityOfWork, IDisposable
    {
        private readonly RoboContext _context;
        private Dictionary<string, object> repositories;

        public IRepository<Robo> Robos { get { return new Repository<Robo>(_context); } }
        public IRepository<Log> Logs { get { return new Repository<Log>(_context); } }
        public UnityOfWork(RoboContext context)
        {
            _context = context;
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Complete()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<T> Repository<T>() where T : EntidadeBase
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;
            if (!repositories.ContainsKey(type))
            {
                var repositorioType = typeof(Repository<>);
                var repositorioInstancia = Activator.CreateInstance(repositorioType.MakeGenericType(typeof(T)), _context);
                repositories.Add(type, repositorioInstancia);
            }

            return (Repository<T>)repositories[type];
        }
    }
}
