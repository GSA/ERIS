using ERIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERIS.Models;
using System.Configuration;
using ERIS.Mapping;

namespace ERIS.Process
{
    internal class ERISSummary
    {
        //Reference to logger
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public readonly SummaryFileGenerator SummaryFileGenerator;
        public List<CreatedSummary> CreatedRecordsProcessed { get; set; }
        public List<UpdatedSummary> UpdatedRecordsProcessed { get; set; }
        public List<FlaggedSummary> FlaggedRecordsProcessed { get; set; }
        public List<ProcessedSummary> SuccessfulProcessed { get; set; }
        public List<ErrorSummary> UnsuccessfulProcessed { get; set; }

        public ERISSummary()
        {
            SummaryFileGenerator = new SummaryFileGenerator();

            SuccessfulProcessed = new List<ProcessedSummary>();
            UnsuccessfulProcessed = new List<ErrorSummary>();
            CreatedRecordsProcessed = new List<CreatedSummary>();
            UpdatedRecordsProcessed = new List<UpdatedSummary>();
            FlaggedRecordsProcessed = new List<FlaggedSummary>();

        }

        public void GenerateSummaryFiles(EMailData emailData)
        {

            if (CreatedRecordsProcessed.Count > 0)
            {
                CreatedRecordsProcessed = CreatedRecordsProcessed.OrderBy(o => o.MonsterID).ToList();

                emailData.CreatedRecordFilename = SummaryFileGenerator.GenerateSummaryFile<CreatedSummary, CreatedSummaryMapping>(ConfigurationManager.AppSettings["CREATEDSUMMARYFILENAME"].ToString(), CreatedRecordsProcessed);

                Log.Info("Created File: " + emailData.CreatedRecordFilename);
            }

            if (UpdatedRecordsProcessed.Count > 0)
            {
                UpdatedRecordsProcessed = UpdatedRecordsProcessed.OrderBy(o => o.MonsterID).ToList();

                emailData.UpdatedRecordFilename = SummaryFileGenerator.GenerateSummaryFile<UpdatedSummary, UpdatedSummaryMapping>(ConfigurationManager.AppSettings["UPDATEDSUMMARYFILENAME"].ToString(), UpdatedRecordsProcessed);

                Log.Info("Updated File: " + emailData.UpdatedRecordFilename);
            }

            if (FlaggedRecordsProcessed.Count > 0)
            {
                FlaggedRecordsProcessed = FlaggedRecordsProcessed.OrderBy(o => o.MonsterID).ToList();

                emailData.FlaggRecordFilename = SummaryFileGenerator.GenerateSummaryFile<FlaggedSummary, FlaggedSummaryMapping>(ConfigurationManager.AppSettings["FLAGGEDSUMMARYFILENAME"].ToString(), FlaggedRecordsProcessed);

                Log.Info("Flagged File: " + emailData.FlaggRecordFilename);
            }

            if (UnsuccessfulProcessed.Count > 0)
            {
                UnsuccessfulProcessed= UnsuccessfulProcessed.OrderBy(o => o.MonsterID).ToList();

                emailData.ErrorFilename= SummaryFileGenerator.GenerateSummaryFile<ErrorSummary, ErrorSummaryMapping>(ConfigurationManager.AppSettings["ERRORSUMMARYFILENAME"].ToString(), UnsuccessfulProcessed);

                Log.Info("Error File: " + emailData.ErrorFilename);
            }



        }

    }
}
