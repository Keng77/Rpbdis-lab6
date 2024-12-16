using Microsoft.AspNetCore.Mvc;
using lab6.Data;
using lab6.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Linq;
using lab6.Models;
using Microsoft.EntityFrameworkCore;

namespace lab6.Controllers
{
    public class InspectionsController : Controller
    {
        private readonly InspectionsDbContext _context;

        public InspectionsController(InspectionsDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}