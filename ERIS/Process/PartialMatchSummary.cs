using ERIS.Models;
using ERIS.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Process
{
    class PartialMatchSummary
    {
        //Reference to logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly EMailData emailData = new EMailData();
        private readonly FlaggedSummary summaryData = new FlaggedSummary();

        public PartialMatchSummary(ref EMailData emailData)
        {
            this.emailData = emailData;
        }

        public void SendSummaryEMail()
        {
            EMail email = new EMail();

            string subject = string.Empty;
            string body = string.Empty;
            string attahcments = string.Empty;

            subject = File.ReadAllText(ConfigurationManager.AppSettings["FLAGGEDSUMMARYEMAILSUBJECT"]);

            subject = subject.Replace("[PROCESSINGDATE]", emailData.ProcessingDate.ToShortDateString());
            subject = subject.Replace("[LAST]", summaryData.LastName);
            subject = subject.Replace("[SUFFIX]", summaryData.Suffix);
            subject = subject.Replace("[FIRST]", summaryData.FirstName);
            subject = subject.Replace("[MIDDLE]", summaryData.MiddleName);

            body = GenerateEMailBody();

            attahcments = SummaryAttachments();

            try
            {
                using (email)
                {
                    email.Send(ConfigurationManager.AppSettings["DEFAULTEMAIL"].ToString(),
                               ConfigurationManager.AppSettings["SUMMARYTO"].ToString(),
                               ConfigurationManager.AppSettings["SUMMARYCC"].ToString(),
                               ConfigurationManager.AppSettings["SUMMARYBCC"].ToString(),
                               subject, body, attahcments.TrimEnd(';'), ConfigurationManager.AppSettings["SMTPSERVER"].ToString(), true);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error Sending ERIS Summary E-Mail: " + ex.Message + " - " + ex.InnerException);
            }
            finally
            {
                log.Info("ERIS Summary E-Mail Sent");
            }
        }

        private string GenerateEMailBody()
        {
            StringBuilder errors = new StringBuilder();
            StringBuilder fileNames = new StringBuilder();

            string template = File.ReadAllText(ConfigurationManager.AppSettings["FLAGGEDSUMMARYTEMPLATE"]);

            template = template.Replace("[LAST]", summaryData.LastName);
            template = template.Replace("[SUFFIX]", summaryData.Suffix);
            template = template.Replace("[FIRST]", summaryData.FirstName);
            template = template.Replace("[MIDDLE]", summaryData.MiddleName);

            return template;
        }

        private string SummaryAttachments()
        {
            StringBuilder attachments = new StringBuilder();

            if (emailData.ErrorFilename != null)
                attachments.Append(AddAttachment(emailData.ErrorFilename));

            return attachments.ToString();
        }

        private string AddAttachment(string fileName)
        {
            StringBuilder addAttachment = new StringBuilder();

            addAttachment.Append(ConfigurationManager.AppSettings["SUMMARYFILEPATH"]);
            addAttachment.Append(fileName);
            addAttachment.Append(";");

            return addAttachment.ToString();
        }
    }
}
