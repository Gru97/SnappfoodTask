namespace Domain;

public class DepositForAShebaRequestDomainService
{
    public void Reserve(Account fromAccount, ShebaRequest shebaRequest)
    {
        if (fromAccount.Balance - shebaRequest.Price < 0)
            throw new DomainException(ErrorMessages.BalanceIsLow);

        fromAccount.Deposit(shebaRequest.Price);
        //TODO: somehow should reserve this in bank account
    }
}