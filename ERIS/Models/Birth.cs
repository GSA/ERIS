using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Models
{
    public class Birth
    {
        public string CityOfBirth { get; set; }
        public string StateOfBirth { get; set; }
        public string CountryOfBirth { get; set; }
        public string CountryOfCitizenship { get; set; }
        public bool Citizen { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
