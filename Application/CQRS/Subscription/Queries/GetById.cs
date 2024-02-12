using Application.Core;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;
using SubscriptionModel = Domain.Models.Subscription;

namespace Application.CQRS.Subscription.Queries
{
    public class GetById
    {
        public class Query : IRequest<Result<SubscriptionModel>>
        {
            public int SubscriptionId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<SubscriptionModel>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IRetryService _retryService;

            public Handler(IUnitOfWork unitOfWork, IRetryService retryService)
            {
                _unitOfWork = unitOfWork;
                _retryService = retryService;
            }

            public async Task<Result<SubscriptionModel>> Handle(Query request, CancellationToken cancellationToken)
            {

                var result = await _retryService.ExecuteWithRetryAsync(() =>
                                                _unitOfWork.Subscriptions.GetAsync(request.SubscriptionId));

                return Result<SubscriptionModel>.Success(result);

            }
        }
    }
}
