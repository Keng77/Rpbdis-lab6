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
    public class InspectorsAPIController : ControllerBase
    {
        private readonly InspectionsDbContext _context;

        public InspectorsAPIController(InspectionsDbContext context)
        {
            _context = context;
        }

        // GET: api/InspectorsAPI
        [HttpGet]
        [SwaggerOperation(Summary = "Получить список инспекторов", Description = "Возвращает список всех инспекторов.")]
        [SwaggerResponse(200, "Список инспекторов успешно получен", typeof(IEnumerable<Inspector>))]
        public async Task<ActionResult<IEnumerable<Inspector>>> GetInspectors()
        {
            return await _context.Inspectors.ToListAsync();
        }

        // GET: api/InspectorsAPI/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Получить инспектора по ID", Description = "Возвращает информацию об инспекторе по его ID.")]
        [SwaggerResponse(200, "Инспектор найден", typeof(Inspector))]
        [SwaggerResponse(404, "Инспектор не найден")]
        public async Task<ActionResult<Inspector>> GetInspector(int id)
        {
            var inspector = await _context.Inspectors.FindAsync(id);

            if (inspector == null)
            {
                return NotFound();
            }

            return inspector;
        }

        // PUT: api/InspectorsAPI/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Обновить информацию об инспекторе", Description = "Обновляет информацию об инспекторе по его ID.")]
        [SwaggerResponse(204, "Информация об инспекторе успешно обновлена")]
        [SwaggerResponse(400, "Некорректный запрос")]
        [SwaggerResponse(404, "Инспектор не найден")]
        public async Task<IActionResult> PutInspector(int id, Inspector inspector)
        {
            if (id != inspector.InspectorId)
            {
                return BadRequest();
            }

            _context.Entry(inspector).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InspectorExists(id))
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

        // POST: api/InspectorsAPI
        [HttpPost]
        [SwaggerOperation(Summary = "Создать нового инспектора", Description = "Создает нового инспектора в базе данных.")]
        [SwaggerResponse(201, "Инспектор успешно создан", typeof(Inspector))]
        public async Task<ActionResult<Inspector>> PostInspector(Inspector inspector)
        {
            _context.Inspectors.Add(inspector);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInspector", new { id = inspector.InspectorId }, inspector);
        }

        // DELETE: api/InspectorsAPI/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Удалить инспектора", Description = "Удаляет инспектора по его ID.")]
        [SwaggerResponse(204, "Инспектор успешно удален")]
        [SwaggerResponse(404, "Инспектор не найден")]
        public async Task<IActionResult> DeleteInspector(int id)
        {
            var inspector = await _context.Inspectors.FindAsync(id);
            if (inspector == null)
            {
                return NotFound();
            }

            _context.Inspectors.Remove(inspector);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InspectorExists(int id)
        {
            return _context.Inspectors.Any(e => e.InspectorId == id);
        }
    }
}