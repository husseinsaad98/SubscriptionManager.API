using MediatR;
using Domain.Repositories;
using Application.Core;
using Application.Interfaces;
using Application.DTOs;

namespace Application.CQRS.User.Queries
{
    public class List
    {
        public class Query : IRequest<Result<List<UserDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<UserDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IRetryService _retryService;

            public Handler(IUnitOfWork unitOfWork, IRetryService retryService)
            {
                _unitOfWork = unitOfWork;
                _retryService = retryService;
            }

            public async Task<Result<List<UserDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _retryService.ExecuteWithRetryAsync(() =>
                                                _unitOfWork.Users.GetAllAsync());

                // we can also user dapper for this
                var users = new List<UserDto>();

                foreach (var user in result)
                {
                    users.Add(new UserDto()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                    });
                }

                return Result<List<UserDto>>.Success(users);
            }
        }
    }
}
