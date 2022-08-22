namespace Pangea.Shared.DataAccess.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        int Complete();
        Task<int> CompleteAsync(CancellationToken cancellationToken);
    }
}
