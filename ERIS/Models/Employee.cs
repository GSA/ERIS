using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MERS.Models
{
    public class Employee
    {
        public Person Person { get; set; }
        public Address Address { get; set; }
        public Building Building { get; set; }
        public Birth Birth { get; set; }
        public Position Position { get; set; }
        public Phone Phone { get; set; }
    }
}
