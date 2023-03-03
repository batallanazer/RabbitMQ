using CrossCuttingLayer;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.RabbitMQ.Publisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IBus _bus;

        public TodoController(IBus bus)
        {
            _bus = bus;
        }
        [HttpPost]
        public async Task<IActionResult> CreateTicket(Todo todoModel)
        {
            if (todoModel is not null)
            {
                Uri uri = new Uri(RabbitMqConsts.RabbitMqUri);
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(todoModel);
                return Ok();
            }
            return BadRequest();
        }
    }
}
