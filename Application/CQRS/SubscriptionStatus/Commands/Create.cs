using Application.Core;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;
using SubscriptionStatusModel = Domain.Models.SubscriptionStatus;

namespace Application.CQRS.SubscriptionStatus.Commands
{
    public class Create
    {
        public class Command : IRequest<Result<SubscriptionStatusModel>>
        {
            public SubscriptionStatusModel SubscriptionStatus { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<SubscriptionStatusModel>>
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

            public async Task<Result<SubscriptionStatusModel>> Handle(Command request, CancellationToken cancellationToken)
            {

                var subscriptionStatus = await _retryService.ExecuteWithRetryAsync(() => _unitOfWork.SubscriptionStatus.AddAsync(request.SubscriptionStatus));

                await _unitOfWork.CompleteAsync();

                return Result<SubscriptionStatusModel>.Success(subscriptionStatus);

            }
        }

    }
}
