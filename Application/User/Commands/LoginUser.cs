using Application.Dto;
using Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Commands
{
    public class LoginUser: IRequest<Result<LogedInUserDto>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
