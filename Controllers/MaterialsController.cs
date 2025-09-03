using MaterialsApp.Data;
using MaterialsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaterialsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialsController(AppDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Material>>> GetAll()
            => await db.Materials.OrderByDescending(m => m.MaterialId).ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Material>> GetOne(int id)
        {
            var item = await db.Materials.FindAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<Material>> Create([FromBody] Material dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // server-side sanity (mirrors frontend rules)
            if (dto.UnitPrice <= 0) ModelState.AddModelError(nameof(dto.UnitPrice), "UnitPrice must be > 0");
            if (dto.GstPercent < 0 || dto.GstPercent > 28) ModelState.AddModelError(nameof(dto.GstPercent), "GstPercent must be 0–28");
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // normalize AddedOn to UTC if client sent local time
            dto.AddedOn = dto.AddedOn == default ? DateTime.UtcNow : dto.AddedOn.ToUniversalTime();

            db.Materials.Add(dto);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOne), new { id = dto.MaterialId }, dto);
        }
    }
}
