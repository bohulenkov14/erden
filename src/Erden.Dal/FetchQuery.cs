namespace Erden.Dal
{
    public abstract class FetchQuery<TResult> : IFetchRequest<TResult>
        where TResult : class
    {
        public FetchQuery(int offset, int size)
        {
            Offset = offset;
            Size = size;
        }

        public int Offset { get; }
        public int Size { get; }
    }
}