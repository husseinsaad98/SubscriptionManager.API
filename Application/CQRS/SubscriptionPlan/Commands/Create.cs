using Application.Core;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;
using SubscriptionPlanModel = Domain.Models.SubscriptionPlan;

namespace Application.CQRS.SubscriptionPlan.Commands
{
    public class Create
    {
        public class Command : IRequest<Result<SubscriptionPlanModel>>
        {
            public SubscriptionPlanModel SubscriptionPlan { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<SubscriptionPlanModel>>
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

            public async Task<Result<SubscriptionPlanModel>> Handle(Command request, CancellationToken cancellationToken)
            {

                var subscriptionPlan = await _retryService.ExecuteWithRetryAsync(() => _unitOfWork.SubscriptionPlans.AddAsync(request.SubscriptionPlan));

                await _unitOfWork.CompleteAsync();

                return Result<SubscriptionPlanModel>.Success(subscriptionPlan);

            }
        }

    }
}
