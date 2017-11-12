namespace Erden.Dal
{
    /// <summary>
    /// Fetch request interface
    /// </summary>
    /// <typeparam name="TResult">Result typr</typeparam>
    public interface IFetchRequest<TResult> where TResult : class { }
}