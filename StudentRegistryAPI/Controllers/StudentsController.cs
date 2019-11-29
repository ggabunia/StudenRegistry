using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistry.BusinessLogic;
using StudentRegistry.DataAccess;
using StudentRegistry.Models;
using StudentRegistryAPI.ViewModels;

namespace StudentRegistryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IUnitOfWork _context;

        public StudentsController(IUnitOfWork context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentVM>>> GetStudents()
        {

            return await Task.FromResult(
                _context.StudentRepository.Get(includeProperties: "Gender").
                Select(s => new StudentVM
                {
                    id = s.Id,
                    firstName = s.FirstName,
                    lastName = s.LastName,
                    pN = s.PersonalNr,
                    DoB = s.BirthDate,
                    genderId = s.GenderID,
                    genderName = s.Gender.GenderName
                }).ToList());
        }

        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<StudentVM>>> GetFilteredStudents(FilterParameters parameters)
        {
            string personalNr = parameters.personalNr;
            DateTime? bDateStart = parameters.bDateStart;
            DateTime? bDateEnd = parameters.bDateEnd;
            var students = _context.StudentRepository.Get(includeProperties: "Gender");
            if (!String.IsNullOrWhiteSpace(personalNr))
            {
                students = students.Where(c => c.PersonalNr.Contains(personalNr));
            }
            if (bDateStart.HasValue)
            {
                students = students.Where(c => c.BirthDate >= bDateStart.Value);
               
            }
            if (bDateEnd.HasValue)
            {
                students = students.Where(c => c.BirthDate <= bDateEnd.Value);             
            }
            return await Task.FromResult(
               students.Select(s => new StudentVM
               {
                   id = s.Id,
                   firstName = s.FirstName,
                   lastName = s.LastName,
                   pN = s.PersonalNr,
                   DoB = s.BirthDate,
                   genderId = s.GenderID,
                   genderName = s.Gender.GenderName
               }).ToList());
        }
        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentVM>> GetStudent(int id)
        {
            var student = await Task.FromResult(_context.StudentRepository.GetByID(id));

            if (student == null)
            {
                return NotFound();
            }
            student.Gender = _context.GenderRepository.GetByID(student.GenderID);
            var result = new StudentVM
            {
                id = student.Id,
                firstName = student.FirstName,
                lastName = student.LastName,
                DoB = student.BirthDate,
                pN = student.PersonalNr,
                genderId = student.GenderID,
                genderName = student.Gender.GenderName

            };
            return result;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, StudentVM student)
        {
            if (id != student.id)
            {
                return BadRequest();
            }
            var stud = _context.StudentRepository.GetByID(id);
            stud.FirstName = student.firstName;
            stud.LastName = student.lastName;
            stud.PersonalNr = student.pN;
            stud.GenderID = student.genderId;
            stud.BirthDate = student.DoB;

            try
            {
                ModelValidator.ValidateStudent(ModelState, stud, _context);
                if (ModelState.IsValid)
                {
                    _context.StudentRepository.Update(stud);
                    _context.Save();
                }
                else
                {
                    var error = new ApiError(400, "Model Validation Failed");
                    var modelErrors = ModelState.Values.Where(x => x.Errors.Count > 0).Select(e => e.Errors).ToList();
                    foreach (var item in modelErrors)
                    {
                        foreach (var er in item)
                        {
                            error.AddError(er.ErrorMessage);
                        }

                    }
                    return new ObjectResult(error);
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(stud.Id))
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
        public async Task<ActionResult<StudentVM>> PostStudent(StudentVM stud)
        {
            Student student = new Student()
            {
                BirthDate = stud.DoB,
                FirstName =stud.firstName,
                GenderID =stud.genderId,
                LastName = stud.lastName,
                PersonalNr = stud.pN
            };
            ModelValidator.ValidateStudent(ModelState, student, _context);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.StudentRepository.Insert(student);
                    _context.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                     throw;
                   
                }
            }
            else
            {
                var error = new ApiError(400, "Model Validation Failed");
                var modelErrors = ModelState.Values.Where(x => x.Errors.Count > 0).Select(e => e.Errors).ToList();
                foreach (var item in modelErrors)
                {
                    foreach (var er in item)
                    {
                        error.AddError(er.ErrorMessage);
                    }

                }
                return new ObjectResult(error);
            }
            stud.id = student.Id;
            return CreatedAtAction("GetStudent", new { id = student.Id }, stud);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            if (!StudentExists(id))
            {
                return NotFound();
            }
            try
            {
                _context.StudentRepository.Delete(id);
                _context.Save();
                return Ok( "სტუდენტი წაიშალა");
            }
            catch (Exception)
            {
                return StatusCode(500,"სტუდენტი ვერ წაიშალა");
            }
        }

        private bool StudentExists(int id)
        {
            return _context.StudentRepository.GetByID(id) != null;
        }
    }
}
