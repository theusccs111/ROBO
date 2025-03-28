using ROBO.Dominio.Entidades;
using ROBO.Dominio.Entidades.Base;
using ROBO.Servico.Interfaces.Repository;

namespace ROBO.Servico.Interfaces
{
    public interface IUnityOfWork : IDisposable
    {
        IRepository<Robo> Robos { get; }
        IRepository<Log> Logs { get; }

        Task CompleteAsync();
        void Complete();
        IRepository<T> Repository<T>() where T : EntidadeBase;
    }
}
