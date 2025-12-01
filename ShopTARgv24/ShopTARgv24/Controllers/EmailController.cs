using Microsoft.AspNetCore.Mvc;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Models.Email;

namespace ShopTARgv24.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailServices _emailService;

        public EmailController
            (
                IEmailServices emailService
            )
        {
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //teha meetod nimega SendEmail, mis võtab vastu EmailDto objekti
        //kasutab EmailServices klassi, et saata email
        [HttpPost]
        public IActionResult SendEmail(EmailViewModel vm)
        {
            var files = Request.Form.Files.Any() ? Request.Form.Files.ToList() : new List<IFormFile>();

            var dto = new EmailDto
            {
                To = vm.To,
                Subject = vm.Subject,
                Body = vm.Body,
                Attachment = files
            };

            _emailService.SendEmail(dto);

            return RedirectToAction(nameof(Index));
        }
    }
}
