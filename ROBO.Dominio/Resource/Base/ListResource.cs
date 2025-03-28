namespace ROBO.Dominio.Resource.Base
{
    public class ListResource<T>
    {
        public IEnumerable<T> Lista { get; set; }
        public long TotalRegistros { get; set; }
    }
}
