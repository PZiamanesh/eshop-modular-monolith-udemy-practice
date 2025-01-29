namespace Shared.Pagination;

public class PaginatedResult<TEnity>
    (int pageIndex, int pageSize, long count, IEnumerable<TEnity> data)
    where TEnity : class
{
    public int PageIndex => pageIndex;
    public int PageSize => pageSize;
    public long Count => count;
    public IEnumerable<TEnity> Data => data;
}
