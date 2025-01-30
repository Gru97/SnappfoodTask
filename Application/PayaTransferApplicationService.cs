using Domain;

namespace Application
{
    public class PayaTransferApplicationService: IPayaTransferApplicationService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IShebaRequestRepository _requestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PayaTransferApplicationService(IAccountRepository accountAccountRepository, IShebaRequestRepository requestRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountAccountRepository;
            _requestRepository = requestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<ShebaRequest>> CreateShebaRequest(ShebaRequestDto dto, CancellationToken token)
        {
            ShebaRequest request;
            try  //this try catch must happen outside here in a general way for all apis and generate non successful status code for client
            {
                var userId = Guid.NewGuid();
                request = ShebaRequest.Create(dto.Price, dto.FromShebaNumber, dto.ToShebaNumber, dto.Note);
                using (var transaction = await _unitOfWork.BeginTransaction())  //implicit lock in db, assuming db is transactional
                {
                    var account = await _accountRepository.GetByUserId(userId, token);
                    var service = new DepositForAShebaRequestDomainService();
                    service.Reserve(account, request);
                    await _requestRepository.Add(request, token);
                    await _accountRepository.Update(account, token);
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                return OperationResult<ShebaRequest>.Failure("10001", "");
            }
            return OperationResult<ShebaRequest>.Success(request, "Successful request");
        }

        public Task AssessShebaRequest(AssessShebaRequestDto dto, CancellationToken token)
        {
            //Unfortunately no time
        }
    }

    public class OperationResult<T>  //we don't write this, usually we use libraries like problem details or fluent result, etc.
    {
        public string? ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
        public T Data { get; set; }

        public static OperationResult<T> Failure(string errorCode, string message) => new OperationResult<T>()
        {
            ErrorCode = errorCode, 
            ErrorMessage = message
        };

        public static OperationResult<T> Success(T data, string message) => new OperationResult<T>()
        {
            Data = data,
            ErrorMessage = message
        };
    }

    public interface IUnitOfWork //TODO : Implement
    {
    }
}
