using Application.Core;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;


namespace Application.CQRS.Subscription.Queries
{
    public class GetSubscriptionRemainingDays
    {

        public class Query : IRequest<Result<int>>
        {
            public int SubscriptionId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IRetryService _retryService;

            public Handler(IUnitOfWork unitOfWork, IRetryService retryService)
            {
                _unitOfWork = unitOfWork;
                _retryService = retryService;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {

                var subscription = await _retryService.ExecuteWithRetryAsync(() =>
                                                _unitOfWork.Subscriptions.GetAsync(request.SubscriptionId));

                if (subscription == null)
                    return Result<int>.Failure("Subscription does not exists");


                TimeSpan remainingTime = subscription.EndDate - DateTime.Now;
                int remainingDays = (int)Math.Ceiling(remainingTime.TotalDays);
                remainingDays = remainingDays > 0 ? remainingDays : 0;
                return Result<int>.Success(remainingDays);

            }

        }
    }
}
