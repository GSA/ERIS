using System;
using System.Diagnostics;
using System.Configuration;
using AutoMapper;
using ERIS.Lookups;
using ERIS.Data;
using ERIS.Mapping;
using ERIS.Process;
using ERIS.Models;

namespace ERIS
{
    internal static class Program
    {
        //Reference to logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //File paths from config file
        //private static string hrFilePath = ConfigurationManager.AppSettings["HRFILE"].ToString();

        //private static string separationFilePath = ConfigurationManager.AppSettings["SEPARATIONFILE"].ToString();

        //Stopwatch objects
        private static Stopwatch timeForApp = new Stopwatch();

        private static Stopwatch timeForProcess = new Stopwatch();

        private static ERISMapper map = new ERISMapper();

        private static IMapper dataMapper;

        private static EMailData emailData = new EMailData();

        static void Main(string[] args)
        {
            SendSummary sendSummary = new SendSummary(ref emailData);
            //Start timer
            timeForApp.Start();

            //Log start of application
            log.Info("Application Started: " + DateTime.Now);

            CreateMaps();

            //Log action
            log.Info("Processing HR Monster File:" + DateTime.Now);

            log.Info("Done Processing HR Links File(s):" + DateTime.Now);

            log.Info("Sending Summary File");
            sendSummary.SendSummaryEMail();
            log.Info("Summary file sent");

            //Stop second timer
            timeForApp.Stop();

            //Log total time
            log.Info(string.Format("Application Completed in {0} milliseconds", timeForApp.ElapsedMilliseconds));

            //Log application end
            log.Info("Application Done: " + DateTime.Now);
        }

        private static void CreateMaps()
        {
            map.CreateDataConfig();
            dataMapper = map.CreateDataMapping();
        }

        private static Lookup createLookups()
        {
            Lookup lookups;
            ERISMapper hrmap = new ERISMapper();
            IMapper lookupMapper;

            hrmap.CreateLookupConfig();

            lookupMapper = hrmap.CreateLookupMapping();

            LoadLookupData loadLookupData = new LoadLookupData(lookupMapper);

            lookups = loadLookupData.GetEmployeeLookupData();

            return lookups;
        }
    }   
}