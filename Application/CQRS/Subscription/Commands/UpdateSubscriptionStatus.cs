using Application.Core;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;
using SubscriptionModel = Domain.Models.Subscription;

namespace Application.CQRS.Subscription.Commands
{
    public class UpdateSubscriptionStatus
    {
        public class Command : IRequest<Result<SubscriptionModel>>
        {
            public int SubscriptionId { get; set; }
            public int StatusId { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<SubscriptionModel>>
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

            public async Task<Result<SubscriptionModel>> Handle(Command request, CancellationToken cancellationToken)
            {

                var subscription = await _retryService.ExecuteWithRetryAsync(() => _unitOfWork.Subscriptions.GetAsync(request.SubscriptionId));

                if (subscription == null)
                    return Result<SubscriptionModel>.Failure("Subscription does not exist");

                subscription.SubscriptionStatusId= request.StatusId;

                _unitOfWork.Subscriptions.Update(subscription);

                await _unitOfWork.CompleteAsync();

                return Result<SubscriptionModel>.Success(subscription);

            }
        }
    }
}
