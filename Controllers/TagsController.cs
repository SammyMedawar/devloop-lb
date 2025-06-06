using DevLoopLB.Models;
using DevLoopLB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevLoopLB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController (ITagService tagService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            var events = await tagService.GetAllTagsAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetTag(int id)
        {
            var tag = await tagService.GetTagByIdAsync(id);
            if(tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTag([FromBody] Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await tagService.AddTagAsync(tag);
            return CreatedAtAction(nameof(GetTag), new { id = tag.TagId }, tag);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTag(int id, [FromBody] Tag tag)
        {
            if (!ModelState.IsValid || id == 0)
            {
                return BadRequest(ModelState);
            }
            tag.TagId = id;
            await tagService.UpdateTagAsync(tag);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTag(int id)
        {
            await tagService.DeleteTagAsync(id);
            return NoContent();
        }
    }
}
