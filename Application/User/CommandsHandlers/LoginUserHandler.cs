using Application.Dto;
using Application.Models;
using Application.Options;
using Application.User.Commands;
using AutoMapper;
using Domain.Aggregates.UserProfileAggregate;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.CommandsHandlers
{
    public class LoginUserHandler : IRequestHandler<LoginUser, Result<LogedInUserDto>>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DataContext _context;
        private Result<LogedInUserDto> _result = new();
        private readonly IMapper _mapper;
        private readonly JwtOptions _jwtOptions;

        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
        public LoginUserHandler(UserManager<IdentityUser> userManager,DataContext context,
            IMapper mapper, IOptions<JwtOptions> jwtOptions)
        {
            _userManager= userManager;
            _context = context;
            _mapper= mapper;
            _jwtOptions = jwtOptions.Value;
        }
        public async Task<Result<LogedInUserDto>> Handle(LoginUser request,
            CancellationToken cancellationToken)
        {
            var user = await getUser(request);
            if (_result.IsError) return _result;
            var userProfile = await _context.userProfiles
                .FirstOrDefaultAsync(p => p.IdentityId == user.Id, cancellationToken);
            _result.Payload = _mapper.Map<LogedInUserDto>(userProfile);
            _result.Payload.UserName = user.UserName;
            _result.Payload.Token = GetJwtToken(user,userProfile);
            return _result;
        }

        private async Task<IdentityUser> getUser(LoginUser request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                _result.IsError = true;
                _result.ErrorType = Enums.ErrorType.Unauthorized;
                _result.Errors.Add("Username or password is incorrect.");  
            }
            var validPassword = await _userManager.CheckPasswordAsync(user,request.Password);
            if (!validPassword)
            {
                _result.IsError = true;
                _result.ErrorType = Enums.ErrorType.Unauthorized;
                _result.Errors.Add("Username or password is incorrect.");
            }
            return user;
        }
        private string GetJwtToken(IdentityUser user, UserProfile userProfile)
        {
            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("UserProfileId",userProfile.Id.ToString())
            });
            var tokenDescriptor = GetTokenDescriptor(claims);
            var token =_tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
        {
            return new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.Now.AddHours(2),
                Audience = _jwtOptions.Audiences[0],
                Issuer = _jwtOptions.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
        }
    }
}
