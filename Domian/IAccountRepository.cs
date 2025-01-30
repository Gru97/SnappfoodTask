namespace Domain;

public interface IAccountRepository
{
    Task<Account> GetByUserId(Guid userId, CancellationToken cancellationToken);
    Task Update(Account account, CancellationToken cancellationToken);
} 

public interface IShebaRequestRepository
{
    Task Add(ShebaRequest request, CancellationToken cancellationToken);
}