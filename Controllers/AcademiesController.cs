using DevLoopLB.DTO;
using DevLoopLB.Models;
using DevLoopLB.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevLoopLB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcademiesController(IAcademyService academyService) : ControllerBase
    {
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Academy>>> GetAllAcademies()
        {
            var academies = await academyService.GetAllAcademiesAsync();
            return Ok(academies);
        }
        [HttpGet("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Academy>> GetAcademy(int id)
        {
            var academy = await academyService.GetAcademyByIdAsync(id);
            return Ok(academy);
        }
        [HttpPost("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateAcademy([FromForm] SaveAcademyDTO academyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var academyId = await academyService.AddAcademyAsync(academyDto);
            return Ok(new { AcademyId = academyId, Message = "Academy created successfully" });
        }

        [HttpPut("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateAcademy(int id, [FromForm] SaveAcademyDTO academyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await academyService.UpdateAcademyAsync(id, academyDto);
            return NoContent();
        }

        [HttpDelete("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAcademy(int id)
        {
            await academyService.DeleteAcademyAsync(id);
            return NoContent();
        }

        [HttpGet("filtered")]
        public async Task<ActionResult<AcademyLoadMoreResponseDTO>> GetPublicAcademies([FromQuery] AcademyFilterRequestDTO filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await academyService.GetFilteredAcademiesAsync(filter);
            return Ok(result);
        }
    }
}
