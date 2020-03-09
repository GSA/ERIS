using System;
using System.Diagnostics;
using System.Configuration;

namespace ERIS
{
    class Program
    {
        //Reference to logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //File paths from config file
        //private static string hrFilePath = ConfigurationManager.AppSettings["HRFILE"].ToString();

        //private static string separationFilePath = ConfigurationManager.AppSettings["SEPARATIONFILE"].ToString();

        //Stopwatch objects
        private static Stopwatch timeForApp = new Stopwatch();

        private static Stopwatch timeForProcess = new Stopwatch();

        public static object ConfigurationManager { get; private set; }

        static void Main(string[] args)
        {
            //Start timer
            timeForApp.Start();

            //Log start of application
            log.Info("Application Started: " + DateTime.Now);

            //Log action
            log.Info("Processing HR Monster File:" + DateTime.Now);

            log.Info("Done Processing HR Links File(s):" + DateTime.Now);

            log.Info("Sending Summary File");
            
            log.Info("Summary file sent");

            //Stop second timer
            timeForApp.Stop();

            //Log total time
            log.Info(string.Format("Application Completed in {0} milliseconds", timeForApp.ElapsedMilliseconds));

            //Log application end
            log.Info("Application Done: " + DateTime.Now);
        }
    }
}
