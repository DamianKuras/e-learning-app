using Application.Enums;
using Application.Models;
using Application.User.Commands;
using Azure.Core;
using Domain.Aggregates.UserProfileAggregate;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User.CommandsHandlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUser, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private Result<Unit> _result = new();
        public RegisterUserHandler(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<Result<Unit>> Handle(RegisterUser request, CancellationToken cancellationToken)
        {

            await validateUserName(request.Username);
            if (_result.IsError) return _result;
            await validateEmail(request.Email);
            if (_result.IsError) return _result;

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var identity = await CreateIdentityUserAsync(request, transaction, cancellationToken);
            if (_result.IsError) return _result;

            await CreateUserProfile(request, transaction, identity, cancellationToken);
            if (_result.IsError) return _result;

            await transaction.CommitAsync();
            return _result;
        }

        private async Task validateUserName(string Username)
        {
            var existingUserWithUsername = await _userManager.FindByNameAsync(Username);
            if (existingUserWithUsername != null)
            {
                _result.IsError = true;
                _result.ErrorType = ErrorType.BadRequest;
                _result.Errors.Add("Username needs to be unique");

            }
        }

        private async Task validateEmail(string Email)
        {
            var existingUserWithEmail = await _userManager.FindByEmailAsync(Email);
            if (existingUserWithEmail != null)
            {
                _result.IsError = true;
                _result.ErrorType = ErrorType.BadRequest;
                _result.Errors.Add("Email addres is already associated with an account. You can only have one account per email addres.");
            }
        }

        private async Task<IdentityUser> CreateIdentityUserAsync(RegisterUser request,
            IDbContextTransaction transaction,
            CancellationToken cancellationToken)
        {
            var identiy = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Username
            };

            var identityUser = await _userManager.CreateAsync(identiy, request.Password);
            if (!identityUser.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                _result.IsError = true;
                foreach (var identityError in identityUser.Errors)
                {
                    _result.IsError = true;
                    _result.ErrorType = ErrorType.BadRequest;
                    _result.Errors.Add(identityError.Description);
                }

            }
            return identiy;
        }

        private async Task CreateUserProfile(RegisterUser request,
             IDbContextTransaction transaction, 
             IdentityUser identityUser,
             CancellationToken cancellationToken)
        {
            try
            {
                var profileBasicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Email);
                var profile = UserProfile.CreateUserProfile(identityUser.Id, profileBasicInfo);
                await _context.userProfiles.AddAsync(profile);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                _result.IsError = true;
                _result.ErrorType = ErrorType.InternalServerError;
                _result.Errors.Add("Internal server error please try again latter");
            }

        }
    }
}
