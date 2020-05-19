using ERIS.Constants;
using ERIS.Models;
using ERIS.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Utilities
{
    class EmailTool
    {
        /*string emailFrom, emailTo, emailCC, emailBCC, server;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        EMail Email = new EMail();
        private bool Debug;

        public EmailTool() { }

        /// <summary>
        /// Set email default values
        /// </summary>
        private void setDefaults()
        {
            emailFrom = "DEFAULTEMAIL".GetSetting();
            emailTo = "TO".GetSetting();
            emailCC = "CC".GetSetting();
            emailBCC = "BCC".GetSetting();
            server = "SMTPSERVER".GetSetting();
        }

        /// <summary>
        /// Uses Email.cs to send email
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private bool SendEmail(string subject, string body, string to, string cc)
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
            EmailCC = cc;
            EmailBCC = emailBCC;

            Email.Send(EmailFrom, EmailTo, EmailCC, EmailBCC, EmailSubject, EmailBody, EmailAttachments, EmailSMTPServer, true);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="contractData"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        private string prepareTo(string to, bool debug)
        {
            to = "";
            return to;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="contractData"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        private string prepareCC(string cc, bool debug)
        {
            return cc;
        }

        /// <summary>
        /// Generates email subject by passing in subject
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="contracts"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        private string prepareEmailSubject(string subject, bool debug)
        {
            string tSubject = subject;

            return tSubject;
        }

        /// <summary>
        /// Generates email body by passing in body
        /// </summary>
        /// <param name="body"></param>
        /// <param name="contracts"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        private string prepareEmailBody(string body, EMailData bodyData, bool debug)
        {
            string tBody = body;

            tBody = tBody.Replace("[PROCESSINGDATE]", bodyData.ProcessingDate.ToString("MM/dd/yyyy"));
            tBody = tBody.Replace("[RECORDSPROCESSED]", bodyData.ItemsProcessed.ToString());
            tBody = tBody.Replace("[RECORDSCREATED]", bodyData.CreateRecord.ToString());
            tBody = tBody.Replace("[RECORDSUPDATED]", bodyData.UpdateRecord.ToString());
            tBody = tBody.Replace("[RECORDSFORHD]", bodyData.ReviewRecord.ToString());
            tBody = tBody.Replace("[ERRORRECORDS]", bodyData.FlagRecord.ToString());

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
                    case Templates.SummaryEmailTemplate:
                        body = File.ReadAllText("SUMMARYFILE".GetSetting());
                        subject = "EMAILSUBJECT".GetSetting();
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
        internal string SendMonsterSummaryEMail(EMailData row)
        {
            string Subject = "", Body = "", To = "", CC = "";

             log.Info("Sending reminder email");
             GetEmailStub(Templates.SummaryEmailTemplate, ref Subject, ref Body);
            

            To = prepareTo(To, Debug);
            Subject = prepareEmailSubject(Subject, Debug);
            Body = prepareEmailBody(Body, row, Debug);
            CC = prepareCC(CC, Debug);

            log.Info("Calling send email function");

            bool Result = SendEmail(Subject, Body, To, CC);

            if (Result)
            {
                log.Info("Email sent successfully! ");
                return prependStatusMessage(Debug, "Email sent successfully!");
            }
            else
            {
                log.Info("Failed to send email!");
                return prependStatusMessage(Debug, "Failed to send email!");
            }

        }*/
    }
}
