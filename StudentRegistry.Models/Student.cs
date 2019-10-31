using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentRegistry.Models
{
    public class Student
    {
     
        public int Id { get; set; }
        [Required(ErrorMessage = "სახელის ველი სავალდებულოა")]
        [Display(Name = "სახელი")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "გვარის ველი სავალდებულოა")]
        [Display(Name = "გვარი")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "დაბადების თარიღის ველი სავალდებულოა")]
        [Display(Name = "დაბადების თარიღი")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "პირადი ნომრის ველი სავალდებულოა")]
        [Display(Name = "პირადი ნომერი")]
        [StringLength(11, MinimumLength = 11, ErrorMessage ="შეიყვანეთ 11 ციფრი")]
        [RegularExpression("^[0-9]{11}", ErrorMessage = "შეიყვანეთ 11 ციფრი")]
        public String PersonalNr { get; set; }


        [ForeignKey("Gender")]
        [Display(Name = "სქესი")]
        [Required(ErrorMessage = "სქესის ველი სავალდებულოა")]
        public int GenderID { get; set; }
        public Gender Gender { get; set; }

    }
}
