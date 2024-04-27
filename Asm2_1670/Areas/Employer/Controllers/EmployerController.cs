using Asm2_1670.Models;
using Asm2_1670.Models.ViewModels;
using Asm2_1670.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asm2_1670.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles ="Employer")]
    public class EmployerController : Controller
    {
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IUnitOfWork _unitOfWork;
        public EmployerController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}


        public IActionResult Profile()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId == null)
            {
                return NotFound();
            }

            Asm2_1670.Models.User? user = _unitOfWork.UsersRepository.Get(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        public IActionResult Profile(Asm2_1670.Models.User user, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                //string wwwrootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string imagesPath = Path.Combine(wwwrootPath, @"images\Employer");
                    if (!String.IsNullOrEmpty(user.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwrootPath, user.ImageUrl.TrimStart('\\'));
						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}
                    using (var fileStream = new FileStream(Path.Combine(imagesPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    user.ImageUrl = @"\images\Employer\" + fileName;
                }
                _unitOfWork.UsersRepository.Update(user);
                _unitOfWork.Save();
				return RedirectToAction("Profile");
			}
            return View(user);
        }
    }
}
