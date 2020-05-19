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

        public List<CreatedSummary> CreatedRecordProcessed { get; set; }
        public List<UpdatedSummary> UpdatedRecordProcessed { get; set; }
        public List<ReviewedSummary> ReviewedRecordProcessed { get; set; }
        public List<FlaggedSummary> FlaggedRecordProcessed { get; set; }

        public ERISSummary()
        {
            SummaryFileGenerator = new SummaryFileGenerator();

            CreatedRecordProcessed = new List<CreatedSummary>();
            UpdatedRecordProcessed = new List<UpdatedSummary>();
            ReviewedRecordProcessed = new List<ReviewedSummary>();
            FlaggedRecordProcessed = new List<FlaggedSummary>();

        }

        public void GenerateSummaryFiles(EMailData emailData)
        {
            if (CreatedRecordProcessed.Count > 0)
            {
                CreatedRecordProcessed = CreatedRecordProcessed.OrderBy(o => o.employeeData.Person.GCIMSID.ToString()).ToList();

                emailData.CreatedRecordFilename = SummaryFileGenerator.GenerateSummaryFile<CreatedSummary, CreatedSummaryMapping>(ConfigurationManager.AppSettings["SUCCESSSUMMARYFILENAME"].ToString(), CreatedRecordProcessed);

                Log.Info("Records Created File: " + emailData.CreatedRecordFilename);
            }

            if (UpdatedRecordProcessed.Count > 0)
            {
                UpdatedRecordProcessed = UpdatedRecordProcessed.OrderBy(o => o.employeeData.Person.GCIMSID.ToString()).ToList();

                emailData.UpdatedRecordFilename = SummaryFileGenerator.GenerateSummaryFile<UpdatedSummary, UpdatedSummaryMapping>(ConfigurationManager.AppSettings["SUCCESSSUMMARYFILENAME"].ToString(), UpdatedRecordProcessed);

                Log.Info("Records Updated File: " + emailData.UpdatedRecordFilename);
            }

            if (ReviewedRecordProcessed.Count > 0)
            {
                ReviewedRecordProcessed = ReviewedRecordProcessed.OrderBy(o => o.employeeData.Person.GCIMSID.ToString()).ToList();

                emailData.ReviewRecordFilename = SummaryFileGenerator.GenerateSummaryFile<ReviewedSummary, ReviewedSummaryMapping>(ConfigurationManager.AppSettings["SUCCESSSUMMARYFILENAME"].ToString(), ReviewedRecordProcessed);

                Log.Info("Records for Review File: " + emailData.ReviewRecordFilename);
            }

            if (FlaggedRecordProcessed.Count > 0)
            {
                FlaggedRecordProcessed = FlaggedRecordProcessed.OrderBy(o => o.employeeData.Person.GCIMSID.ToString()).ToList();

                emailData.FlaggRecordFilename = SummaryFileGenerator.GenerateSummaryFile<FlaggedSummary, FlaggedSummaryMapping>(ConfigurationManager.AppSettings["SUCCESSSUMMARYFILENAME"].ToString(), FlaggedRecordProcessed);

                Log.Info("Records Flagged File: " + emailData.FlaggRecordFilename);
            }



        }

    }
}
