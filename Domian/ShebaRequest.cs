namespace Domain;

public class ShebaRequest
{
    public Guid Id { get; private set; }
    public long Price { get; private set; }
    public Sheba From { get; private set; }
    public Sheba To { get; private set; }
    public string Note { get; private set; }
    public RequestState State { get; private set; }
    public DateTime CreateAt { get; private set; }
    private ShebaRequest(long price, Sheba from, Sheba to, string note)
    {
        Price = price;
        From = from;
        To = to;
        Note = note;
        State = RequestState.Pending;
        CreateAt = DateTime.Now;
        Id=Guid.NewGuid();
    }

    public static ShebaRequest Create(string price, string from, string to, string note)
    {
        if(!long.TryParse(price, out var priceInt)) //Can be a validation in application layer
            throw new DomainException(ErrorMessages.InvalidPrice);

        if (priceInt == 0)
            throw new DomainException(ErrorMessages.InvalidPrice);

        var fromSheba = new Sheba(from);
        var toSheba = new Sheba(to);
        return new ShebaRequest(priceInt, fromSheba, toSheba, note);
    }
}

public enum RequestState
{
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2
}

public class Sheba
{
    public string Value { get; }

    public Sheba(string value)
    {
        if (value.Length != 26 || !value.StartsWith("IR"))
            throw new DomainException(ErrorMessages.InvalidSheba);
        Value = value;
    }
}

public class Account
{
    private Account(Guid userId, long initialBalance)
    {
        UserId = userId;
        Balance = initialBalance;
        Id=Guid.NewGuid();
    }

    public long Balance { get; private set; }
    public Guid UserId { get; private set; }
    public Guid Id { get; private set; }

    public static Account Create(Guid userId, long initialBalance)
    {
        return new Account(userId, initialBalance);
    }
    public void Deposit(long amount)
    {
        Balance -= amount;
    }
}