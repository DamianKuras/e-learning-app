using Api.Contracts.User;
using Application.Enums;
using Application.Models.Result;
using Application.User.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    public class UserController : Controller
    {
        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var command = _mapper.Map<LoginUser>(login);
            var result = await _mediator.Send(command);
            if (!result.IsSucces)
            {
                HandleErrors(result.Status, result.Errors);
            }
            return Ok(_mapper.Map<User>(result.Payload));
        }

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register([FromBody] Register registration)
        {
            var command = _mapper.Map<RegisterUser>(registration);
            var result = await _mediator.Send(command);
            if (!result.IsSucces)
            {
                HandleErrors(result.Status, result.Errors);
            }
            return Accepted();
        }
    }
}