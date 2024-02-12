using Application.Core;
using Application.DTOs;
using Application.Interfaces;
using Domain.Models;
using Domain.Repositories;
using MediatR;
using UserModel = Domain.Models.User;

namespace Application.CQRS.User.Queries
{
    public class GetById
    {
        public class Query : IRequest<Result<UserDto>>
        {
            public string UserId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<UserDto>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IRetryService _retryService;

            public Handler(IUnitOfWork unitOfWork, IRetryService retryService)
            {
                _unitOfWork = unitOfWork;
                _retryService = retryService;
            }

            public async Task<Result<UserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _retryService.ExecuteWithRetryAsync(() =>
                                                _unitOfWork.Users.GetByIdAsync(request.UserId));

                if (user == null)
                    return Result<UserDto>.Failure("User does not exist");

                return Result<UserDto>.Success(new UserDto()
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                });
            }
        }
    }

}
