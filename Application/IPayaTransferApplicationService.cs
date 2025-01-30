using Domain;

namespace Application;

public interface IPayaTransferApplicationService
{
    Task<OperationResult<ShebaRequest>> CreateShebaRequest(ShebaRequestDto dto, CancellationToken token);
    Task AssessShebaRequest(AssessShebaRequestDto dto, CancellationToken token);
}

public record ShebaRequestDto(string FromShebaNumber, string ToShebaNumber, string Price, string Note);
public record AssessShebaRequestDto(Guid RequestId, string Note, int State);