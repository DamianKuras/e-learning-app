using Application.Models.Result;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class Controller : ControllerBase
    {
        private IMapper _mapperInstance;
        private IMediator _mediatorInstance;
        protected IMapper _mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();
        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();

        protected IActionResult HandleErrors(Status status, List<string> Errors)

        {
            if (status == Status.NotFound)
            {
                return NotFound(Errors);
            }
            if (status == Status.Forbidden)
            {
                return Forbid();
            }
            if (status == Status.Unauthorized)
            {
                return Unauthorized(Errors);
            }
            return BadRequest(Errors);
        }
    }
}