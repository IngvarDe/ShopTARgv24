using Microsoft.AspNetCore.Mvc;
using ReactCRUD.Data;
using ReactCRUD.Server.ViewModels;

namespace ReactCRUD.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ReactCRUDContext _context;

        public SchoolController
            (
                ReactCRUDContext context
            )
        {
            _context = context;
        }

        [HttpGet(Name = "SchoolList")]
        public IActionResult Index()
        {
            var result = _context.Schools
                .Select(x => new SchoolListViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    StudentCount = x.StudentCount,
                });

            return Ok(result);
        }
    }
}
