using ERIS.Constants;
using ERIS.Models;
using ERIS.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERIS.Utilities
{
    class ReviewSummary
    {
        string emailFrom, emailTo, emailCC, emailBCC, server;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        EMail Email = new EMail();
        private bool Debug;


        public ReviewSummary() { }

        /// <summary>
        /// Set email default values
        /// </summary>
        private void setDefaults()
        {
            emailFrom = "REVIEWSUMMARYFROM".GetSetting();
           //emailTo = "TO".GetSetting();
            emailCC = "REVIEWSUMMARYCC".GetSetting();
            emailBCC = "REVIEWSUMMARYBCC".GetSetting();
            server = "SMTPSERVER".GetSetting();
        }

        /// <summary>
        /// Uses Email.cs to send email
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private bool SendEmail(string subject, string body, string to)
        {

            log.Info("Configuring variables for email");

            setDefaults();

            string EmailTo, EmailCC, EmailBCC, EmailFrom, EmailSubject, EmailBody, EmailAttachments, EmailSMTPServer;

            EmailSubject = subject;
            EmailBody = body;
            EmailAttachments = "";
            EmailSMTPServer = server;
            EmailFrom = emailFrom;
            EmailTo = to;
            EmailCC = emailCC;
            EmailBCC = emailBCC;

            Email.Send(EmailFrom, EmailTo, EmailCC, EmailBCC, EmailSubject, EmailBody, EmailAttachments, EmailSMTPServer, true);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="summaryData"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        private string prepareTo(string to, FlaggedSummary summaryData, bool debug)
        {
            to = ConfigurationManager.AppSettings["REVIEWSUMMARYTO"].ToString();
            to = to.Replace("[HREMAIL]", summaryData.HREmail);
            return to;
        }

        /// <summary>
        /// Generates email subject by passing in subject
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="summaryData"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        private string prepareEmailSubject(string subject, FlaggedSummary summaryData, bool debug)
        {
            string tSubject = subject;

            tSubject = tSubject.Replace("[PROCESSINGDATE]", DateTime.Now.ToString("MM/dd/yyyy"));
            tSubject = tSubject.Replace("[LAST]", summaryData.LastName);
            tSubject = tSubject.Replace("[SUFFIX]", summaryData.Suffix);
            tSubject = tSubject.Replace("[FIRST]", summaryData.FirstName);
            tSubject = tSubject.Replace("[MIDDLE]", summaryData.MiddleName);

            if (summaryData.Suffix == "")
            {
                tSubject = Regex.Replace(tSubject, " *,", ",");
            }

            return tSubject;
        }

        /// <summary>
        /// Generates email body by passing in body
        /// </summary>
        /// <param name="body"></param>
        /// <param name="summaryData"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        private string prepareEmailBody(string body, FlaggedSummary summaryData, bool debug)
        {
            string tBody = body;

            tBody = tBody.Replace("[LAST]", summaryData.LastName);
            tBody = tBody.Replace("[SUFFIX]", summaryData.Suffix);
            tBody = tBody.Replace("[FIRST]", summaryData.FirstName);
            tBody = tBody.Replace("[MIDDLE]", summaryData.MiddleName);
            tBody = tBody.Replace("[RUNDATE]", DateTime.Now.ToString("MM/dd/yyyy"));
            tBody = tBody.Replace("[MONSTERID]", summaryData.MonsterID);
            tBody = tBody.Replace("[MATCHINGFIELDS]", summaryData.MatchingFields);

            return tBody;
        }

        /// <summary>
        /// Gets email subject and body by ref
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        internal bool GetEmailStub(string templateName, ref string subject, ref string body)
        {
            //string Folder = "TEMAILTEMPLATE".GetSetting();
            try
            {
                switch (templateName)
                {
                    case Templates.ReviewSummaryEmailTemplate:
                        body = File.ReadAllText("FLAGGEDSUMMARYTEMPLATE".GetSetting());
                        subject = "FLAGGEDSUMMARYEMAILSUBJECT".GetSetting();
                        break;
                    default:
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return false;
            }

        }

        /// <summary>
        /// Prepend debug message to string
        /// </summary>
        /// <param name="debug"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string prependStatusMessage(bool debug, string msg)
        {
            return msg.Prepend(debug ? "**DEBUG** " : "");
        }

        /// <summary>
        /// Create Monster summary email 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        internal string SendReviewSummaryEMail(FlaggedSummary row)
        {
            string Subject = "", Body = "", To = "";

             log.Info("Sending review email to HR");
             GetEmailStub(Templates.ReviewSummaryEmailTemplate, ref Subject, ref Body);
            

            To = prepareTo(To, row, Debug);
            Subject = prepareEmailSubject(Subject, row, Debug);
            Body = prepareEmailBody(Body, row, Debug);

            //log.Info("Calling send email function");

            bool Result = SendEmail(Subject, Body, To);

            if (Result)
            {
                //log.Info("Email sent successfully! ");
                return prependStatusMessage(Debug, "Email sent successfully!");
            }
            else
            {
                //log.Info("Failed to send email!");
                return prependStatusMessage(Debug, "Failed to send email!");
            }

        }
    }
}
