using Application.Core;
using Application.DTOs;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;
using UserModel = Domain.Models.User;

namespace Application.CQRS.User.Commands
{
    public class Create
    {
        public class Command : IRequest<Result<AuthenticationResponse>>
        {
            public UserDto User { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<AuthenticationResponse>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IJwtProvider _jwtProvider;
            private readonly IRetryService _retryService;

            public Handler(
              IUnitOfWork unitOfWork,
              IRetryService retryService,
              IJwtProvider jwtProvider)
            {
                _unitOfWork = unitOfWork;
                _retryService = retryService;
                _jwtProvider = jwtProvider;
            }

            public async Task<Result<AuthenticationResponse>> Handle(Command request, CancellationToken cancellationToken)
            {

                //we can user mapper for this aswell
                var user = new UserModel()
                {
                    FirstName = request.User.FirstName,
                    LastName = request.User.LastName,
                    Email = request.User.Email,
                    UserName = request.User.Email,
                    PasswordHash = request.User.Password,
                    PhoneNumber = request.User.PhoneNumber
                };

                var result = await _retryService.ExecuteWithRetryAsync(() => _unitOfWork.Users.Add(user));

                if (!result.Succeeded)
                    return Result<AuthenticationResponse>.Failure(result.Errors.First().Description);

                string token = _jwtProvider.Generate(user, null);

                return Result<AuthenticationResponse>.Success(new AuthenticationResponse()
                {
                    Expiration = DateTime.UtcNow.AddDays(1),
                    Token = token
                });

            }
        }
    }
}
