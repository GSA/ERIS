using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Models
{
    class EMailData
    {
        public string MonsterFilename { get; set; }

        public DateTime ProcessingDate { get; set; }
        public Int64 ItemsProcessed { get; set; }
        public Int64 MonsterSucceeded { get; set; }
        public Int64 CreateRecord { get; set; }
        public Int64 UpdateRecord { get; set; }
        public Int64 FlagRecord { get; set; }
        public Int64 MonsterFailed{ get; set; }

        public string CreatedRecordFilename { get; set; }
        public string UpdatedRecordFilename { get; set; }
        public string FlaggRecordFilename { get; set; }
        public string ErrorFilename { get; set; }
    }
}
