﻿using DevLoopLB.DTO;
using DevLoopLB.Models;
using DevLoopLB.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DevLoopLB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController(IEventService eventService) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await eventService.GetAllEventsAsync();
            return Ok(events);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateEvent([FromForm] SaveEventDTO evt)
        {
            if (!ModelState.IsValid || !evt.Gallery.Any() || !TryValidateModel(evt.Gallery, "SaveImageAssetDTO"))
            {
                return BadRequest(ModelState);
            }
            var createdEventID = await eventService.AddEventAsync(evt);
            return Ok(new { EventId = createdEventID, Message = "Event created successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            await eventService.DeleteEventAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateEvent (int id, [FromForm] SaveEventDTO evt)
        {
            if (!ModelState.IsValid || !evt.Gallery.Any() || !TryValidateModel(evt.Gallery, "SaveImageAssetDTO"))
            {
                return BadRequest(ModelState);
            }
            await eventService.UpdateEventAsync(id, evt);
            return NoContent();
        }

        [HttpGet("filtered")]
        [EnableRateLimiting("EventFilterPolicy")]
        public async Task<ActionResult<EventPagedResponseDTO>> GetFilteredEvents([FromQuery] EventFilterRequestDTO filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await eventService.GetFilteredEventsAsync(filter);
            return Ok(result);
        }
    }
}
