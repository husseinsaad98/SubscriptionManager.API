using Application.Core;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Subscription.Commands
{
    public class Delete
    {
        public class Command : IRequest<Result<string>>
        {
            public int SubscriptionId { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<string>>
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

            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {

                var subscription = await _retryService.ExecuteWithRetryAsync(() => _unitOfWork.Subscriptions.GetAsync(request.SubscriptionId));

                if (subscription == null)
                    return Result<string>.Failure("Subscription does not exist");

                _unitOfWork.Subscriptions.Remove(subscription);

                await _unitOfWork.CompleteAsync();

                return Result<string>.Success("Deleted succesfully");

            }
        }
    }
}
