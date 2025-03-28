namespace ROBO.Dominio.Resource.Base
{
    public class PaginatedResource
    {
        public string? SortFieldId { get; set; }
        public string? SortOrder { get; set; }
        public int Page { get; set; }
        public int QuantityRecords { get; set; }
    }
}
