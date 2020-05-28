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
        public List<ReviewedSummary> ReviewedRecordsProcessed { get; set; }
        public List<FlaggedSummary> FlaggedRecordsProcessed { get; set; }
        public List<ProcessedSummary> SuccessfulUsersProcessed { get; set; }
        public List<ProcessedSummary> UnsuccessfulUsersProcessed { get; set; }

        public ERISSummary()
        {
            SummaryFileGenerator = new SummaryFileGenerator();

            SuccessfulUsersProcessed = new List<ProcessedSummary>();
            UnsuccessfulUsersProcessed = new List<ProcessedSummary>();
            CreatedRecordsProcessed = new List<CreatedSummary>();
            UpdatedRecordsProcessed = new List<UpdatedSummary>();
            ReviewedRecordsProcessed = new List<ReviewedSummary>();
            FlaggedRecordsProcessed = new List<FlaggedSummary>();

        }

        public void GenerateSummaryFiles(EMailData emailData)
        {

            if (CreatedRecordsProcessed.Count > 0)
            {
                CreatedRecordsProcessed = CreatedRecordsProcessed.OrderBy(o => o.LastName).ThenBy(t => t.FirstName).ToList();

                emailData.CreatedRecordFilename = SummaryFileGenerator.GenerateSummaryFile<CreatedSummary, CreatedSummaryMapping>(ConfigurationManager.AppSettings["CREATEDSUMMARYFILENAME"].ToString(), CreatedRecordsProcessed);

                Log.Info("Created File: " + emailData.CreatedRecordFilename);
            }

            if (UpdatedRecordsProcessed.Count > 0)
            {
                UpdatedRecordsProcessed = UpdatedRecordsProcessed.OrderBy(o => o.LastName).ThenBy(t => t.FirstName).ToList();

                emailData.UpdatedRecordFilename = SummaryFileGenerator.GenerateSummaryFile<UpdatedSummary, UpdatedSummaryMapping>(ConfigurationManager.AppSettings["UPDATEDSUMMARYFILENAME"].ToString(), UpdatedRecordsProcessed);

                Log.Info("Updated File: " + emailData.UpdatedRecordFilename);
            }

            if (ReviewedRecordsProcessed.Count > 0)
            {
                ReviewedRecordsProcessed = ReviewedRecordsProcessed.OrderBy(o => o.LastName).ThenBy(t => t.FirstName).ToList();

                emailData.ReviewRecordFilename = SummaryFileGenerator.GenerateSummaryFile<ReviewedSummary, ReviewedSummaryMapping>(ConfigurationManager.AppSettings["REVIEWEDSUMMARYFILENAME"].ToString(), ReviewedRecordsProcessed);

                Log.Info("Reviewed File: " + emailData.ReviewRecordFilename);
            }

            if (FlaggedRecordsProcessed.Count > 0)
            {
                FlaggedRecordsProcessed = FlaggedRecordsProcessed.OrderBy(o => o.LastName).ThenBy(t => t.FirstName).ToList();

                emailData.FlaggRecordFilename = SummaryFileGenerator.GenerateSummaryFile<FlaggedSummary, FlaggedSummaryMapping>(ConfigurationManager.AppSettings["FLAGGEDSUMMARYFILENAME"].ToString(), FlaggedRecordsProcessed);

                Log.Info("Flagged File: " + emailData.FlaggRecordFilename);
            }



        }

    }
}
