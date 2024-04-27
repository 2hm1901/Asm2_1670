using Asm2_1670.Data;
using Asm2_1670.Models;
using Asm2_1670.Repository;
using Asm2_1670.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asm2_1670.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CategoriesController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public CategoriesController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
		{
			List<Categories> myList = _unitOfWork.CategoriesRepository.GetAll().ToList();
			return View(myList);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(Categories categories)
		{
			if(ModelState.IsValid)
			{
				_unitOfWork.CategoriesRepository.Add(categories);
				_unitOfWork.Save();
				return RedirectToAction("Index");
			}
			return View();
		}
		public IActionResult Edit(int? id)
		{
			if(id == null || id == 0)
			{
				return NotFound();
			}
			Categories? category = _unitOfWork.CategoriesRepository.Get(c => c.Id == id);
			if(category == null)
			{
				return NotFound();
			}
			return View(category);
		}
		[HttpPost]
		public IActionResult Edit(Categories category)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.CategoriesRepository.Update(category);
				_unitOfWork.Save();
				return RedirectToAction("Index");
			}
			return View();
		}
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Categories? category = _unitOfWork.CategoriesRepository.Get(c => c.Id == id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}
		[HttpPost]
		public IActionResult Delete(Categories category)
		{
			_unitOfWork.CategoriesRepository.Delete(category);
			_unitOfWork.Save();
			return RedirectToAction("Index");
		}
	}
	
}
