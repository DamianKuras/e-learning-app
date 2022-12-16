using Application.Dto;
using Application.Models;
using MediatR;

namespace Application.User.Commands
{
    public class LoginUser : IRequest<Result<LogedInUserDto>>
    {
        public string Password { get; set; }
        public string Username { get; set; }
    }
}