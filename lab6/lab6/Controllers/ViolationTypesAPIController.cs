using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab6.Data;
using lab6.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace lab6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViolationTypesAPIController : ControllerBase
    {
        private readonly InspectionsDbContext _context;

        public ViolationTypesAPIController(InspectionsDbContext context)
        {
            _context = context;
        }

        // GET: api/ViolationTypesAPI
        [HttpGet]
        [SwaggerOperation(Summary = "Получить список типов нарушений", Description = "Возвращает список всех типов нарушений.")]
        [SwaggerResponse(200, "Список типов нарушений успешно получен", typeof(IEnumerable<ViolationType>))]
        public async Task<ActionResult<IEnumerable<ViolationType>>> GetViolationTypes()
        {
            return await _context.ViolationTypes.ToListAsync();
        }

        // GET: api/ViolationTypesAPI/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Получить тип нарушения по ID", Description = "Возвращает информацию о типе нарушения по его ID.")]
        [SwaggerResponse(200, "Тип нарушения найден", typeof(ViolationType))]
        [SwaggerResponse(404, "Тип нарушения не найден")]
        public async Task<ActionResult<ViolationType>> GetViolationType(int id)
        {
            var violationType = await _context.ViolationTypes.FindAsync(id);

            if (violationType == null)
            {
                return NotFound();
            }

            return violationType;
        }

        // PUT: api/ViolationTypesAPI/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Обновить тип нарушения", Description = "Обновляет информацию о типе нарушения по его ID.")]
        [SwaggerResponse(204, "Тип нарушения успешно обновлен")]
        [SwaggerResponse(400, "Некорректный запрос")]
        [SwaggerResponse(404, "Тип нарушения не найден")]
        public async Task<IActionResult> PutViolationType(int id, ViolationType violationType)
        {
            if (id != violationType.ViolationTypeId)
            {
                return BadRequest();
            }

            _context.Entry(violationType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ViolationTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ViolationTypesAPI
        [HttpPost]
        [SwaggerOperation(Summary = "Создать новый тип нарушения", Description = "Создает новый тип нарушения в базе данных.")]
        [SwaggerResponse(201, "Тип нарушения успешно создан", typeof(ViolationType))]
        public async Task<ActionResult<ViolationType>> PostViolationType(ViolationType violationType)
        {
            _context.ViolationTypes.Add(violationType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetViolationType", new { id = violationType.ViolationTypeId }, violationType);
        }

        // DELETE: api/ViolationTypesAPI/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Удалить тип нарушения", Description = "Удаляет тип нарушения по его ID.")]
        [SwaggerResponse(204, "Тип нарушения успешно удален")]
        [SwaggerResponse(404, "Тип нарушения не найден")]
        public async Task<IActionResult> DeleteViolationType(int id)
        {
            var violationType = await _context.ViolationTypes.FindAsync(id);
            if (violationType == null)
            {
                return NotFound();
            }

            _context.ViolationTypes.Remove(violationType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ViolationTypeExists(int id)
        {
            return _context.ViolationTypes.Any(e => e.ViolationTypeId == id);
        }
    }
}