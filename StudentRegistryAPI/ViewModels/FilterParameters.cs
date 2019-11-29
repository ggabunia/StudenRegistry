using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistryAPI.ViewModels
{
    public class FilterParameters
    {
        public string personalNr { get; set; } 
        public DateTime? bDateStart { get; set; }

        public DateTime? bDateEnd { get; set; }
    }
}
