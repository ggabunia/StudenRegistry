using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentRegistry.DataAccess;
using StudentRegistry.Models;
using StudentRegistry.BusinessLogic;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace StudentRegistry.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string currentPnFilter, string personalNr, DateTime? bDateStart, 
            DateTime? currentdStartFilter, DateTime? bDateEnd, DateTime? currentdEndFilter, int? pageNumber)
        {
            if (personalNr != null || bDateStart != null || bDateEnd!=null)
            {
                pageNumber = 1;
            }
            else
            {
                personalNr = currentPnFilter;
                bDateStart = currentdStartFilter;
                bDateEnd = currentdEndFilter;
            }
            ViewData["CurrentPnFilter"] = personalNr;
            var students = _unitOfWork.StudentRepository.Get(includeProperties: "Gender");
            if (!String.IsNullOrWhiteSpace(personalNr))
            {
                students = students.Where(c => c.PersonalNr.Contains(personalNr));
            }
            if (bDateStart.HasValue)
            {
                students = students.Where(c => c.BirthDate>= bDateStart.Value);
                ViewData["CurrentdStartFilter"] = bDateStart.Value.ToString("yyyy-MM-dd");
            }
            if (bDateEnd.HasValue)
            {
                students = students.Where(c => c.BirthDate <= bDateEnd.Value);
                ViewData["CurrentdEndFilter"] = bDateEnd.Value.ToString("yyyy-MM-dd");
            }

            int pageSize = 5;
            return View(PaginatedList<Student>.GetList(students, pageNumber ?? 1, pageSize));
        }

        public IActionResult CreateUpdate(int? id)
        {
            if (id == null)
            {
                ViewData["GenderID"] = new SelectList(_unitOfWork.GenderRepository.Get(), "ID", "GenderName");
                ViewBag.ActionName = "create";
                ViewBag.Heading = "სტუდენტის დამატება";
                return View();
            }
            else
            {
                var student = _unitOfWork.StudentRepository.GetByID(id);
                if (student == null)
                {
                    return NotFound();
                }
                ViewData["GenderID"] = new SelectList(_unitOfWork.GenderRepository.Get(), "ID", "GenderName", student.GenderID);
                ViewBag.ActionName = "edit";
                ViewBag.Heading = "სტუდენტის რედაქტირება";
                ViewBag.Edit = true;
                return View(student);
            }



        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,FirstName,LastName,BirthDate,PersonalNr,GenderID")] Student student)
        {

            ModelValidator.ValidateStudent(ModelState, student, _unitOfWork);
            if (ModelState.IsValid)
            {
                _unitOfWork.StudentRepository.Insert(student);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ActionName = "create";
            ViewBag.Heading = "სტუდენტის დამატება";
            ViewData["GenderID"] = new SelectList(_unitOfWork.GenderRepository.Get(), "ID", "GenderName", student.GenderID);
            return View("CreateUpdate", student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,FirstName,LastName,BirthDate,PersonalNr,GenderID")] Student student)
        {
            ViewBag.Edit = true;
            if (id != student.Id)
            {
                return NotFound();
            }

            ModelValidator.ValidateStudent(ModelState, student, _unitOfWork);
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.StudentRepository.Update(student);
                    _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenderID"] = new SelectList(_unitOfWork.GenderRepository.Get(), "ID", "GenderName", student.GenderID);
            ViewBag.ActionName = "edit";
            ViewBag.Heading = "სტუდენტის რედაქტირება";
            return View("CreateUpdate",student);
        }




        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _unitOfWork.StudentRepository.Delete(id);
                _unitOfWork.Save();
                return Content("სტუდენტი წაიშალა");
            }
            catch (Exception)
            {
                return Content("წაშლა ვერ მოხერხდა");
            }
        }

        private bool StudentExists(int id)
        {
            return _unitOfWork.StudentRepository.GetByID(id) != null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
