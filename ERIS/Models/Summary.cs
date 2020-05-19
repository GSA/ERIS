using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Models
{
    /*internal class ProcessedSummary
    {
        //TODO: Add Summary items here
        //This is an exmample of an item   
        
                public DateTime ProcessingDate { get; set; }
                public Int64 ItemsProcessed { get; set; }
                public Int64 CreateRecord { get; set; }
                public Int64 UpdateRecord { get; set; }
                public Int64 FlagRecord { get; set; }
                public Int64 ErrorRecord { get; set; }

       
    }*/

    internal class CreatedSummary
    {
        public Employee employeeData { get; set; }
    }

    internal class UpdatedSummary
    {
        public Employee employeeData { get; set; }
    }

    internal class ReviewedSummary
    {
        public Employee employeeData { get; set; }
    }

    internal class FlaggedSummary
    {
        public Employee employeeData { get; set; }
    }
}
