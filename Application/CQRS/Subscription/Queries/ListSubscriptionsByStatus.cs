using Application.Core;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;
using SubscriptionModel = Domain.Models.Subscription;


namespace Application.CQRS.Subscription.Queries
{
    public class ListSubscriptionsByStatus
    {

        public class Query : IRequest<Result<List<SubscriptionModel>>>
        {
            public int StatusId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<SubscriptionModel>>>
        {
            private readonly IRetryService _retryService;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IRetryService retryService, IUnitOfWork unitOfWork)
            {
                _retryService = retryService;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<List<SubscriptionModel>>> Handle(Query request, CancellationToken cancellationToken)
            {


                var result = await _retryService.ExecuteWithRetryAsync(() =>
                                                _unitOfWork.Subscriptions.FindAsync(x => x.SubscriptionStatusId == request.StatusId));

                return Result<List<SubscriptionModel>>.Success(result.ToList());

            }
        }
    }
}
