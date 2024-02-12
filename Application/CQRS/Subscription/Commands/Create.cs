using Application.Core;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;
using SubscriptionModel = Domain.Models.Subscription;

namespace Application.CQRS.Subscription.Commands
{
    public class Create
    {
        public class Command : IRequest<Result<SubscriptionModel>>
        {
            public SubscriptionModel Subscription { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<SubscriptionModel>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IRetryService _retryService;

            public Handler(
              IUnitOfWork unitOfWork,
              IRetryService retryService)
            {
                _unitOfWork = unitOfWork;
                _retryService = retryService;
            }

            public async Task<Result<SubscriptionModel>> Handle(Command request, CancellationToken cancellationToken)
            {

                request.Subscription.SubscriptionStatusId = (int)Domain.Enums.Subscription.SubscriptionStatus.Active;

                var subscription = await _retryService.ExecuteWithRetryAsync(() => _unitOfWork.Subscriptions.AddAsync(request.Subscription));

                await _unitOfWork.CompleteAsync();

                return Result<SubscriptionModel>.Success(subscription);

            }
        }

    }
}
