using Application.Core;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;

namespace Application.CQRS.SubscriptionPlan.Commands
{
    public class Delete
    {
        public class Command : IRequest<Result<string>>
        {
            public int SubscriptionPlanId { get; set; }
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
                var subscriptionPlan = await _retryService.ExecuteWithRetryAsync(() => _unitOfWork.SubscriptionPlans.GetAsync(request.SubscriptionPlanId));

                if (subscriptionPlan == null)
                    return Result<string>.Failure("Subscription plan does not exist");

                _unitOfWork.SubscriptionPlans.Remove(subscriptionPlan);

                await _unitOfWork.CompleteAsync();

                return Result<string>.Success("Deleted succesfully");

            }
        }
    }
}
