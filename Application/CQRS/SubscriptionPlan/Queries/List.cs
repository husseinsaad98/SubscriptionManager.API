using Application.Core;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;
using SubscriptionPlanModel = Domain.Models.SubscriptionPlan;

namespace Application.CQRS.SubscriptionPlan.Queries
{
    public class List
    {
        public class Query : IRequest<Result<List<SubscriptionPlanModel>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<SubscriptionPlanModel>>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IRetryService _retryService;

            public Handler(IUnitOfWork unitOfWork, IRetryService retryService)
            {
                _unitOfWork = unitOfWork;
                _retryService = retryService;
            }

            public async Task<Result<List<SubscriptionPlanModel>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var result = await _retryService.ExecuteWithRetryAsync(() =>
                                                _unitOfWork.SubscriptionPlans.GetAllAsync());

                return Result<List<SubscriptionPlanModel>>.Success(result.ToList());

            }
        }
    }
}
