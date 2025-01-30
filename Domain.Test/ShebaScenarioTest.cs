using System.Security.Principal;

namespace Domain.Test;

public class ShebaScenarioTest
{
    [Fact]
    public void Should_Successfully_Create_Sheba_Request()
    {
        var request = CreateShebaRequest();
        Assert.NotNull(request); //and other field values ... 
    }

    private ShebaRequest CreateShebaRequest()
    {
        long price = 200000000;
        var from = "IR330620000000202901868005";
        var to = "IR330620000000202901868006";
        var note = "توضیحات تراکنش";

        var request = ShebaRequest.Create(price, from, to, note);
        return request;
    }

    [Fact]
    public void Should_Fail_To_Create_Sheba_Request_If_Sheba_Is_Not_Valid()
    {
        var price = 200000000;
        var from = "IR3306200000002029018680050"; //27
        var to = "IR330620000000202901868006";
        var note = "توضیحات تراکنش";

        var request= () => ShebaRequest.Create(price, from, to, note);

        var exception= Assert.Throws<DomainException>(request);
        Assert.Equal(ErrorMessages.InvalidSheba, exception.Message);
    }

    [Fact]
    public void Should_Reserve_Money()
    {
        var userId = Guid.NewGuid();
        var request = CreateShebaRequest();

        var account = Account.Create(userId, request.Price);
        var currentBalance = account.Balance;
        var service = new DepositForAShebaRequestDomainService();
        service.Reserve(account, request);

        Assert.Equal(currentBalance- request.Price, account.Balance);

    }
}

