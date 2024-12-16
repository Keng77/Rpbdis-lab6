using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab6.Data;
using lab6.Models;
using Swashbuckle.AspNetCore.Annotations;
using lab6.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;  // Для аннотаций Swagger

namespace lab6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionsAPIController : ControllerBase
    {
        private readonly InspectionsDbContext _context;

        public InspectionsAPIController(InspectionsDbContext context)
        {
            _context = context;
        }

        // GET: api/InspectionsAPI
        [HttpGet]
        [SwaggerOperation(Summary = "Получить список проверок", Description = "Возвращает список всех проверок.")]
        [SwaggerResponse(200, "Список проверок успешно получен", typeof(IEnumerable<Inspection>))]
        public async Task<ActionResult<IEnumerable<Inspection>>> GetInspections()
        {
            // Жадная загрузка связанных сущностей
            return await _context.Inspections
                .Include(i => i.Inspector)      // Загрузка инспектора
                .Include(i => i.Enterprise)      // Загрузка предприятия
                .Include(i => i.ViolationType)   // Загрузка типа нарушения
                .ToListAsync();
        }

        // GET: api/InspectionsAPI/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Получить проверку по ID", Description = "Возвращает информацию о проверке по ее ID.")]
        [SwaggerResponse(200, "Проверка найдена", typeof(Inspection))]
        [SwaggerResponse(404, "Проверка не найдена")]
        public async Task<ActionResult<Inspection>> GetInspection(int id)
        {
            var inspection = await _context.Inspections
                .Include(i => i.Inspector)      // Жадная загрузка для проверки по ID
                .Include(i => i.Enterprise)
                .Include(i => i.ViolationType)
                .FirstOrDefaultAsync(i => i.InspectionId == id);

            if (inspection == null)
            {
                return NotFound();
            }

            return inspection;
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Обновить информацию о проверке", Description = "Обновляет информацию о проверке по ее ID.")]
        [SwaggerResponse(204, "Информация о проверке успешно обновлена")]
        [SwaggerResponse(400, "Некорректный запрос")]
        [SwaggerResponse(404, "Проверка не найдена")]
        public async Task<IActionResult> PutInspection(int id, InspectionDto inspectionDto)
        {
            if (id != inspectionDto.InspectionId)
            {
                return BadRequest();
            }

            // Преобразуем DTO в модель
            var inspection = new Inspection
            {
                InspectionId = inspectionDto.InspectionId,
                InspectorId = inspectionDto.InspectorId,
                EnterpriseId = inspectionDto.EnterpriseId,
                ViolationTypeId = inspectionDto.ViolationTypeId,
                InspectionDate = inspectionDto.InspectionDate,
                ProtocolNumber = inspectionDto.ProtocolNumber,
                ResponsiblePerson = inspectionDto.ResponsiblePerson,
                PenaltyAmount = inspectionDto.PenaltyAmount,
                PaymentDeadline = inspectionDto.PaymentDeadline,
                CorrectionDeadline = inspectionDto.CorrectionDeadline,
                PaymentStatus = inspectionDto.PaymentStatus,
                CorrectionStatus = inspectionDto.CorrectionStatus
            };

            _context.Entry(inspection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InspectionExists(id))
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

        [HttpPost]
        [SwaggerOperation(Summary = "Создать новую проверку", Description = "Создает новую проверку в базе данных.")]
        [SwaggerResponse(201, "Проверка успешно создана", typeof(Inspection))]
        public async Task<ActionResult<Inspection>> PostInspection(CreateInspectionDto inspectionDto)
        {
            // Преобразуем DTO в модель
            var inspection = new Inspection
            {
                InspectorId = inspectionDto.InspectorId,
                EnterpriseId = inspectionDto.EnterpriseId,
                ViolationTypeId = inspectionDto.ViolationTypeId,
                InspectionDate = inspectionDto.InspectionDate,
                ProtocolNumber = inspectionDto.ProtocolNumber,
                ResponsiblePerson = inspectionDto.ResponsiblePerson,
                PenaltyAmount = inspectionDto.PenaltyAmount,
                PaymentDeadline = inspectionDto.PaymentDeadline,
                CorrectionDeadline = inspectionDto.CorrectionDeadline,
                PaymentStatus = inspectionDto.PaymentStatus,
                CorrectionStatus = inspectionDto.CorrectionStatus
            };

            _context.Inspections.Add(inspection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInspection", new { id = inspection.InspectionId }, inspection);
        }

        // DELETE: api/InspectionsAPI/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Удалить проверку", Description = "Удаляет проверку по ее ID.")]
        [SwaggerResponse(204, "Проверка успешно удалена")]
        [SwaggerResponse(404, "Проверка не найдена")]
        public async Task<IActionResult> DeleteInspection(int id)
        {
            var inspection = await _context.Inspections.FindAsync(id);
            if (inspection == null)
            {
                return NotFound();
            }

            _context.Inspections.Remove(inspection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InspectionExists(int id)
        {
            return _context.Inspections.Any(e => e.InspectionId == id);
        }

    }
}