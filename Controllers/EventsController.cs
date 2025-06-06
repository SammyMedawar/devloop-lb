using DevLoopLB.Models;
using DevLoopLB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevLoopLB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController(IEventService eventService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await eventService.GetAllEventsAsync();
            return Ok(events);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var evt = await eventService.GetEventByIdAsync(id);
            if (evt == null)
            {
                return NotFound();
            }
            return Ok(evt);
        }
        [HttpPost]
        public async Task<ActionResult> CreateEvent([FromBody] Event evt)
        {
            await eventService.AddEventAsync(evt);
            return CreatedAtAction(nameof(GetEvent), new { id = evt.EventId }, evt);
        }
    }
}
