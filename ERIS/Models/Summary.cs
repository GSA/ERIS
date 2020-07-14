using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Models
{
    internal class ProcessedSummary
    {
        public string MonsterID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string Action { get; set; }
    }

    internal class CreatedSummary
    {
        public string MonsterID { get; set; }    
        public Int64 GCIMSID { get; set; }    
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
    }

    internal class UpdatedSummary
    {
        public string MonsterID { get; set; }
        public Int64 GCIMSID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public int Sponsored { get; set; }
        public string UpdatedFields { get; set; }
    }

    internal class FlaggedSummary
    {
        public string MonsterID { get; set; }
        //public Int64 GCIMSID { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string MatchingFields { get; set; }
        public string HREmail { get; set; }
    }

    internal class ErrorSummary
    {
        public string MonsterID { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string ValidationErrors { get; set; }
    }
}
