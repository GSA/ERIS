using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Models
{
    public class Person
    {
        public Int64 GCIMSID { get; set; } //If Matched we set this
        public string MonsterID { get; set; }        
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }       
        public string Suffix { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string Gender { get; set; }
        public string HomeEmail { get; set; }
    }
}
