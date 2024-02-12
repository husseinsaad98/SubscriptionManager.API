using Application.Core;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;

namespace Application.CQRS.SubscriptionStatus.Commands
{
    public class Delete
    {
        public class Command : IRequest<Result<string>>
        {
            public int SubscriptionStatusId { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<string>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IRetryService _retryService;

            public Handler(
              IUnitOfWork unitOfWor,
              IRetryService retryService)
            {
                _unitOfWork = unitOfWor;
                _retryService = retryService;
            }

            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {

                var subscriptionStatus = await _retryService.ExecuteWithRetryAsync(() => _unitOfWork.SubscriptionStatus.GetAsync(request.SubscriptionStatusId));

                if (subscriptionStatus == null)
                    return Result<string>.Failure("Subscription status does not exist");

                _unitOfWork.SubscriptionStatus.Remove(subscriptionStatus);

                await _unitOfWork.CompleteAsync();

                return Result<string>.Success("Deleted succesfully");

            }
        }
    }
}
