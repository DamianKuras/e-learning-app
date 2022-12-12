using Api.Contracts.User;
using Application.Enums;
using Application.User.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public UserController(IMediator mediator,IMapper mapper)
        {
            _mediator= mediator;
            _mapper= mapper;
        }


        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register([FromBody]Register registration)
        {
            var command = _mapper.Map<RegisterUser>(registration);
            var result = await _mediator.Send(command);
            if (result.IsError)
            {
                if (result.ErrorType == ErrorType.BadRequest)
                {
                    return BadRequest(result.Errors);
                } 
                if(result.ErrorType == ErrorType.InternalServerError)
                {
                    return StatusCode(500,result.Errors);
                }
            }
            return Accepted();
        }
        


    }
}
