using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistryAPI.ViewModels
{
    public class StudentVM
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string pN { get; set; }

        public DateTime DoB { get; set; }
        public int genderId { get; set; }
        public string genderName { get; set; }
    }
}
