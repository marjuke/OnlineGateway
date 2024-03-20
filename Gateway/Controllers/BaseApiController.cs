using Application.StanService;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseApiController: ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        //public BaseApiController()
        //{
        //     Mediator.Send(new StanUpdate.Command { Stan = "OK" });
        //}
    }
}
