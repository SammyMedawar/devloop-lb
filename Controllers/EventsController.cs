﻿using DevLoopLB.DTO;
using DevLoopLB.Models;
using DevLoopLB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public async Task<ActionResult> CreateEvent([FromBody] SaveEventDTO evt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdEventID = await eventService.AddEventAsync(evt);
            return CreatedAtAction(nameof(GetEvent), new { createdEventID });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            await eventService.DeleteEventAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEvent (int id, [FromBody] SaveEventDTO evt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await eventService.UpdateEventAsync(id, evt);
            return NoContent();
        }
    }
}
