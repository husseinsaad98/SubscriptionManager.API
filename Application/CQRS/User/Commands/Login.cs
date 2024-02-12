using Application.Core;
using Application.DTOs;
using Application.Interfaces;
using Domain.Repositories;
using MediatR;
using System.Data;
using UserModel = Domain.Models.User;

namespace Application.CQRS.User.Commands;

public sealed class Login
{
    public class Command : IRequest<Result<AuthenticationResponse>>
    {
        public UserCredentials UserCredentials { get; set; }
    }
    public class Handler : IRequestHandler<Command, Result<AuthenticationResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRetryService _retryService;

        public Handler(
          IUnitOfWork unitOfWork,
          IJwtProvider jwtProvider,
          IRetryService retryService)
        {
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
            _retryService = retryService;
        }

        public async Task<Result<AuthenticationResponse>> Handle(Command request, CancellationToken cancellationToken)
        {

            bool isAuthenticated = await _retryService.ExecuteWithRetryAsync(() =>
                                                       _unitOfWork.Users.IsUserAuthenticated(request.UserCredentials.Email, request.UserCredentials.Password));

            if (!isAuthenticated)
                return Result<AuthenticationResponse>.Failure("Wrong username or password");

            UserModel user = await _retryService.ExecuteWithRetryAsync(() =>
                                                 _unitOfWork.Users.GetByEmailAsync(request.UserCredentials.Email));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var userClaims = await _retryService.ExecuteWithRetryAsync(() =>
                                              _unitOfWork.Users.GetUserClaims(user));

            string token = _jwtProvider.Generate(user, userClaims.ToList());

            return Result<AuthenticationResponse>.Success(new AuthenticationResponse()
            {
                Expiration = DateTime.UtcNow.AddDays(1),
                Token = token
            });

        }
    }
}

