namespace MoneyRemittance.BuildingBlocks.Application.Contracts;

public abstract record Query<TResult> : Query, IQuery<TResult>
{
    public Guid InternalProcessId { get; }

    protected Query()
    {
        InternalProcessId = Guid.NewGuid();
    }
}

public abstract record Query
{
    internal Query()
    {

    }
}

public abstract record PageableQuery<TResult> : Query<PagedDto<TResult>>
{
    public int PageNumber { get; }
    public int PageSize { get; }

    public PageableQuery(int pageNumber, int pageSize)
        : base()
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class PagedDto<T>
{
    public PagedDto() { }

    public T[] Items { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}
