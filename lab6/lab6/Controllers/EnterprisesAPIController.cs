using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab6.Data;
using lab6.Models;
using Swashbuckle.AspNetCore.Annotations;  // Для аннотаций Swagger

namespace lab6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnterprisesAPIController : ControllerBase
    {
        private readonly InspectionsDbContext _context;

        public EnterprisesAPIController(InspectionsDbContext context)
        {
            _context = context;
        }

        // GET: api/Enterprises
        [HttpGet]
        [SwaggerOperation(Summary = "Получить список предприятий", Description = "Возвращает список всех предприятий.")]
        [SwaggerResponse(200, "Список предприятий успешно получен", typeof(IEnumerable<Enterprise>))]
        public async Task<ActionResult<IEnumerable<Enterprise>>> GetEnterprises()
        {
            return await _context.Enterprises.ToListAsync();
        }

        // GET: api/Enterprises/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Получить предприятие по ID", Description = "Возвращает информацию о предприятии по его ID.")]
        [SwaggerResponse(200, "Предприятие найдено", typeof(Enterprise))]
        [SwaggerResponse(404, "Предприятие не найдено")]
        public async Task<ActionResult<Enterprise>> GetEnterprise(int id)
        {
            var enterprise = await _context.Enterprises.FindAsync(id);

            if (enterprise == null)
            {
                return NotFound();
            }

            return enterprise;
        }

        // PUT: api/Enterprises/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Обновить информацию о предприятии", Description = "Обновляет информацию о предприятии по его ID.")]
        [SwaggerResponse(204, "Информация о предприятии успешно обновлена")]
        [SwaggerResponse(400, "Некорректный запрос")]
        [SwaggerResponse(404, "Предприятие не найдено")]
        public async Task<IActionResult> PutEnterprise(int id, Enterprise enterprise)
        {
            if (id != enterprise.EnterpriseId)
            {
                return BadRequest();
            }

            _context.Entry(enterprise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnterpriseExists(id))
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

        // POST: api/Enterprises
        [HttpPost]
        [SwaggerOperation(Summary = "Создать новое предприятие", Description = "Создает новое предприятие в базе данных.")]
        [SwaggerResponse(201, "Предприятие успешно создано", typeof(Enterprise))]
        public async Task<ActionResult<Enterprise>> PostEnterprise(Enterprise enterprise)
        {
            _context.Enterprises.Add(enterprise);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnterprise", new { id = enterprise.EnterpriseId }, enterprise);
        }

        // DELETE: api/Enterprises/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Удалить предприятие", Description = "Удаляет предприятие по его ID.")]
        [SwaggerResponse(204, "Предприятие успешно удалено")]
        [SwaggerResponse(404, "Предприятие не найдено")]
        public async Task<IActionResult> DeleteEnterprise(int id)
        {
            var enterprise = await _context.Enterprises.FindAsync(id);
            if (enterprise == null)
            {
                return NotFound();
            }

            _context.Enterprises.Remove(enterprise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EnterpriseExists(int id)
        {
            return _context.Enterprises.Any(e => e.EnterpriseId == id);
        }
    }
}