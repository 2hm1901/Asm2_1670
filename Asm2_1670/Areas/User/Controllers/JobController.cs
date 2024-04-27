using Asm2_1670.Models;
using Asm2_1670.Models.ViewModels;
using Asm2_1670.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Asm2_1670.Areas.User.Controllers
{
    [Area("User")]
    public class JobController : Controller
    {
		private readonly IUnitOfWork _unitOfWork;
		public JobController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public string TakeIdUser()
		{
			var claimIdentity = (ClaimsIdentity)User.Identity;
			string userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			return userId;
		}
		public Asm2_1670.Models.User TakeUser(string userId)
		{
			Asm2_1670.Models.User? user = _unitOfWork.UsersRepository.Get(u => u.Id == userId);

			ViewBag.UserId = userId;
			ViewBag.Email = user.Email;
			ViewBag.Name = user.Name;
			return user;
		}
		public IActionResult Index()
        {
			
			string UserId = TakeIdUser();
			ViewBag.UserId = UserId;
			JobAppVM jobAppVM = new JobAppVM()
			{
				Application = new Application(),
				MyListJob = _unitOfWork.JobsRepository.GetAll().ToList(),
				MyListApplication = _unitOfWork.ApplicationsRepository.GetAll().ToList()
			};
			return View(jobAppVM);
        }
		[HttpPost] 
		public IActionResult Index(JobAppVM jobAppVM)
		{
			string currentTime = DateTime.Now.ToShortDateString();
			jobAppVM.Application.AppliedTime = currentTime;
			_unitOfWork.ApplicationsRepository.Add(jobAppVM.Application);
			_unitOfWork.Save();

			string UserId = TakeIdUser();
			ViewBag.UserId = UserId;
			jobAppVM.MyListJob = _unitOfWork.JobsRepository.GetAll().ToList();
			jobAppVM.MyListApplication = _unitOfWork.ApplicationsRepository.GetAll().ToList();
			return View(jobAppVM);
		}
        public IActionResult Jobdetail(int? id)
        {
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Job? job = _unitOfWork.JobsRepository.Get(j => j.Id == id);
			if (job == null)
			{
				return NotFound();
			}
			return View(job);
        }
    }
}
