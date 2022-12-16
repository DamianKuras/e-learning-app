using Application.Models;
using MediatR;

namespace Application.User.Commands
{
    public class RegisterUser : IRequest<Result<Unit>>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}