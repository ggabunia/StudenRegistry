using System;
using System.Collections.Generic;
using System.Text;
using StudentRegistry.Models;
using System.Linq;

namespace StudentRegistry.BusinessLogic
{
    public class ModelValidator
    {
        public static void ValidateStudent(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState, Student student, IUnitOfWork unitOfWork)
        {
            student.FirstName = student.FirstName.Trim();
            student.LastName = student.LastName.Trim();
            student.PersonalNr = student.PersonalNr.Trim();

            if (!String.IsNullOrWhiteSpace(student.PersonalNr) && unitOfWork.StudentRepository.Get(filter: c => c.PersonalNr == student.PersonalNr && c.Id != student.Id).Count() > 0)
            {
                modelState.AddModelError("PersonalNr", "ეს პირადი ნომერი უკვე დარეგისტრირებულია");
            }

            if (student.BirthDate.Date.AddYears(16) > DateTime.Now.Date)
            {
                modelState.AddModelError("BirthDate", "სტუდენტი არ შეიძლება იყოს 16 წელზე პატარა");
            }
        }


    }
}
