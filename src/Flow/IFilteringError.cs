namespace Flow
{
    public interface IFilteringError
    {
        int Code { get; }
        string Message { get; }
    }
}