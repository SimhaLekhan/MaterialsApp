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
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Material dto)
        {
            if (id != dto.MaterialId) return BadRequest();

            var existing = await db.Materials.FindAsync(id);
            if (existing == null) return NotFound();

            // Prevent duplicate Name
            if (await db.Materials.AnyAsync(m => m.Name == dto.Name && m.MaterialId != id))
                return Conflict(new { message = "Material name must be unique." });

            // Update fields
            existing.Name = dto.Name;
            existing.Category = dto.Category;
            existing.Brand = dto.Brand;
            existing.UnitPrice = dto.UnitPrice;
            existing.UnitOfMeasure = dto.UnitOfMeasure;
            existing.InStockQty = dto.InStockQty;
            existing.ReorderLevel = dto.ReorderLevel;
            existing.GstPercent = dto.GstPercent;
            existing.IsActive = dto.IsActive;

            await db.SaveChangesAsync();
            return NoContent();
        }

        // ✅ DELETE
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await db.Materials.FindAsync(id);
            if (item == null) return NotFound();

            db.Materials.Remove(item);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}
