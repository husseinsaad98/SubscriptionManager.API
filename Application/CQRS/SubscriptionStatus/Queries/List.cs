using Application.Core;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;
using SubscriptionStatusModel = Domain.Models.SubscriptionStatus;

namespace Application.CQRS.SubscriptionStatus.Queries
{
    public class List
    {
        public class Query : IRequest<Result<List<SubscriptionStatusModel>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<SubscriptionStatusModel>>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IRetryService _retryService;

            public Handler(IUnitOfWork unitOfWork, IRetryService retryService)
            {
                _unitOfWork = unitOfWork;
                _retryService = retryService;
            }

            public async Task<Result<List<SubscriptionStatusModel>>> Handle(Query request, CancellationToken cancellationToken)
            {


                var result = await _retryService.ExecuteWithRetryAsync(() =>
                                                _unitOfWork.SubscriptionStatus.GetAllAsync());

                return Result<List<SubscriptionStatusModel>>.Success(result.ToList());

            }
        }
    }
}
