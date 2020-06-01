using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using ERIS.Data;
using ERIS.Lookups;
using ERIS.Mapping;
using ERIS.Models;
using ERIS.Utilities;
using ERIS.Validation;
using FluentValidation.Results;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Process
{

    class ProcessMonster
    {
        //Reference to logger
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private static CsvConfiguration config;
        private readonly RetrieveData retrieve;
        private static readonly string[] TerritoriesNotCountriesArray = new string[] { "rq", "gq", "vq", "aq" };
        readonly Lookup lookups;
        private readonly EMailData emailData;

        //Constructor
        public ProcessMonster(IMapper dataMapper, ref EMailData emailData, Lookup lookups)
        {
            retrieve = new RetrieveData(dataMapper);

            this.lookups = lookups;

            this.emailData = emailData;
        }

        public void ProcessMonsterFile(string MonsterFile)
        {
            log.Info("Processing Monster File");

            try
            {
                //TODO: Add code here to process monster file
                Employee gcimsRecord;

                var summary = new ERISSummary();
                var fileReader = new FileReader();
                var validate = new ValidateMonster(lookups);
                var em = new EmployeeMapping();
                List<string> badRecords;

                log.Info("Loading Monster File");             
                               
                var MonsterData = fileReader.GetFileData<Employee, EmployeeMapping>(MonsterFile, out badRecords, em);

                //Helpers.AddBadRecordsToSummary(badRecords, ref summary);

                log.Info("Loading GCIMS Data");
                var allGCIMSData = retrieve.AllGCIMSData();

                //ProcessResult updatedResults;

                //Start Processing the Monster Data
                foreach (Employee employeeData in MonsterData)
                {
                    log.Info("Processing Data: " + employeeData.Person.MonsterID);

                    ////Looking for matching record.
                    //log.Info("Looking for matching record: " + employeeData.Person.MonsterID);
                    //gcimsRecord = Helpers.RecordFound(employeeData, allGCIMSData, ref log);

                    //if ((gcimsRecord != null && (gcimsRecord.Person.GCIMSID != employeeData.Person.GCIMSID) && (!Convert.ToBoolean(ConfigurationManager.AppSettings["DEBUG"].ToString()))))
                    //{
                    //    log.Info("Adding Monster to record: " + gcimsRecord.Person.MonsterID);
                    //    //save.InsertEmployeeID(gcimsRecord.Person.GCIMSID, employeeData.Person.EmployeeID);
                    //}

                    ////If no record found write to the createdd summary file
                    //if (gcimsRecord == null)
                    //{
                    //    //Danger Will Robinson, Danger
                    //    summary.CreatedRecordsProcessed.Add(new CreatedSummary
                    //    {
                    //        MonsterID = -1,
                    //        FirstName = employeeData.Person.FirstName,
                    //        MiddleName = employeeData.Person.MiddleName,
                    //        LastName = employeeData.Person.LastName,
                    //        SocialSecurityNumber = employeeData.Person.SocialSecurityNumber,
                    //        DateOfBirth = employeeData.Birth.DateOfBirth

                    //    });
                    //}

                    if (TerritoriesNotCountriesArray.Contains(employeeData.Birth.CountryOfBirth.ToLower()) && string.IsNullOrWhiteSpace(employeeData.Birth.StateOfBirth))
                    {
                        switch (employeeData.Birth.CountryOfBirth.ToLower())
                        {
                            case "rq":
                                { employeeData.Birth.StateOfBirth = "PR"; }
                                break;
                            case "gq":
                                { employeeData.Birth.StateOfBirth = "GU"; }
                                break;
                            case "vq":
                                { employeeData.Birth.StateOfBirth = "VI"; }
                                break;
                            case "aq":
                                { employeeData.Birth.StateOfBirth = "AS"; }
                                break;
                            default:
                                { employeeData.Birth.StateOfBirth = ""; }
                                break;
                        }
                        employeeData.Birth.CountryOfBirth = "US";
                    }

                    //If there are critical errors write to the error summary and move to the next record
                    log.Info("Checking for Critical errors for user: " + employeeData.Person.MonsterID);
                    if (Helpers.CheckForErrors(validate, employeeData, summary.UnsuccessfulProcessed, ref log))
                        continue;

                    Helpers.CleanupMonsterData(employeeData);
           #region test
                    //    //If DB Record is not null them check if we need to update record
                    //    if (gcimsRecord != null)
                    //    {
                    //        //Hold the exclude list
                    //        var excludeList = new List<string>();
                    //        excludeList.AddRange(new[] { "InitialResult", "InitialResultDate", "FinalResult", "FinalResultDate" });                        

                    //        //Run personal phone number copy logic
                    //        var personalExcludeList = new[] { "HomePhone", "PersonalCell", "WorkCell" };
                    //        excludeList.AddRange(personalExcludeList);
                    //        //var eft = new ExcludedFieldTool("Phone");
                    //        //eft.Create(
                    //        //    "Phone",
                    //        //    personalExcludeList,
                    //        //    employeeData
                    //        //);
                    //        //eft.Process(employeeData, gcimsRecord);

                    //        log.Info("Comparing Monster and GCIMS Data: " + employeeData.Person.MonsterID);
                    //        if (!AreEqualGCIMSToHR(gcimsRecord, employeeData, out columnList))
                    //        {
                    //            //Checking if the SSN are different
                    //            if (employeeData.Person.SocialSecurityNumber != gcimsRecord.Person.SocialSecurityNumber)
                    //            {
                    //                summary.SocialSecurityNumberChanges.Add(new SocialSecurityNumberChangeSummary
                    //                {
                    //                    GCIMSID = gcimsRecord.Person.GCIMSID,
                    //                    EmployeeID = employeeData.Person.EmployeeID,
                    //                    FirstName = employeeData.Person.FirstName,
                    //                    MiddleName = employeeData.Person.MiddleName,
                    //                    LastName = employeeData.Person.LastName,
                    //                    Suffix = employeeData.Person.Suffix,
                    //                    Status = gcimsRecord.Person.Status
                    //                });
                    //            }

                    //            log.Info("Copying objects: " + employeeData.Person.EmployeeID);
                    //            helper.CopyValues<Employee>(employeeData, gcimsRecord, new string[] { "InitialResult", "InitialResultDate", "FinalResult", "FinalResultDate" });

                    //            log.Info("Checking if inactive record: " + employeeData.Person.EmployeeID);

                    //            if (employeeData.Person.Status == "Inactive")
                    //            {
                    //                summary.InactiveRecords.Add(new InactiveSummary
                    //                {
                    //                    GCIMSID = gcimsRecord.Person.GCIMSID,
                    //                    EmployeeID = employeeData.Person.EmployeeID,
                    //                    FirstName = employeeData.Person.FirstName,
                    //                    MiddleName = employeeData.Person.MiddleName,
                    //                    LastName = employeeData.Person.LastName,
                    //                    Suffix = employeeData.Person.Suffix,
                    //                    Status = employeeData.Person.Status
                    //                });

                    //                log.Warn("Inactive Record: " + employeeData.Person.EmployeeID);
                    //            }

                    //            log.Info("Updating Record: " + employeeData.Person.EmployeeID);

                    //            if (Convert.ToBoolean(ConfigurationManager.AppSettings["DEBUG"].ToString()))
                    //            {
                    //                updatedResults = new ProcessResult
                    //                {
                    //                    Result = -1,
                    //                    Action = "Testing",
                    //                    Error = "SQL Error (Testing)"
                    //                };
                    //            }
                    //            else
                    //            {
                    //                updatedResults = save.UpdatePersonInformation(gcimsRecord.Person.GCIMSID, employeeData);
                    //            }

                    //            if (updatedResults.Result > 0)
                    //            {
                    //                summary.SuccessfulUsersProcessed.Add(new ProcessedSummary
                    //                {
                    //                    GCIMSID = gcimsRecord.Person.GCIMSID,
                    //                    EmployeeID = employeeData.Person.EmployeeID,
                    //                    FirstName = employeeData.Person.FirstName,
                    //                    MiddleName = employeeData.Person.MiddleName,
                    //                    LastName = employeeData.Person.LastName,
                    //                    Suffix = employeeData.Person.Suffix,
                    //                    Action = updatedResults.Action,
                    //                    UpdatedColumns = columnList
                    //                });

                    //                log.Info("Successfully Updated Record: " + employeeData.Person.EmployeeID);
                    //            }
                    //            else
                    //            {
                    //                summary.UnsuccessfulUsersProcessed.Add(new ProcessedSummary
                    //                {
                    //                    GCIMSID = gcimsRecord.Person.GCIMSID,
                    //                    EmployeeID = employeeData.Person.EmployeeID,
                    //                    FirstName = employeeData.Person.FirstName,
                    //                    MiddleName = employeeData.Person.MiddleName,
                    //                    LastName = employeeData.Person.LastName,
                    //                    Suffix = employeeData.Person.Suffix,
                    //                    Status = employeeData.Person.Status,
                    //                    Action = updatedResults.Error
                    //                });

                    //                log.Error("Unable to update: " + employeeData.Person.GCIMSID);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            log.Info("Monster and GCIMS Data are the same: " + employeeData.Person.GCIMSID);

                    //            summary.IdenticalRecords.Add(new IdenticalRecordSummary
                    //            {
                    //                GCIMSID = gcimsRecord.Person.GCIMSID,
                    //                EmployeeID = employeeData.Person.EmployeeID,
                    //                FirstName = employeeData.Person.FirstName,
                    //                MiddleName = employeeData.Person.MiddleName,
                    //                LastName = employeeData.Person.LastName,
                    //                Suffix = employeeData.Person.Suffix,
                    //                Status = gcimsRecord.Person.Status
                    //            });
                    //        }
                    //    }
                    //}
                    #endregion test                    
                }

                emailData.MonsterFilename = Path.GetFileName(MonsterFile);
                emailData.ItemsProcessed = MonsterData.Count;
                //emailData.CreateRecord = summary.CreatedRecordsProcessed.Count;
                //emailData.UpdateRecord = summary.UpdatedRecordsProcessed.Count;
                //emailData.ReviewRecord = summary.ReviewedRecordsProcessed.Count;
                //emailData.FlagRecord = summary.FlaggedRecordsProcessed.Count;
                emailData.MonsterFailed = summary.UnsuccessfulProcessed.Count;

                //Add log entries
                log.Info("Total records " + String.Format("{0:#,###0}", MonsterData.Count));
                //log.Info("Records created: " + String.Format("{0:#,###0}", summary.CreatedRecordsProcessed.Count));
                //log.Info("Records updated: " + String.Format("{0:#,###0}", summary.UpdatedRecordsProcessed.Count));
                //log.Info("Records reviewed: " + String.Format("{0:#,###0}", summary.ReviewedRecordsProcessed.Count));
                //log.Info("Records flagged: " + String.Format("{0:#,###0}", summary.FlaggedRecordsProcessed.Count));
                log.Info("Records Invalid: " + String.Format("{0:#,###0}", summary.UnsuccessfulProcessed.Count));

                summary.GenerateSummaryFiles(emailData);
            }
            //Catch all errors

            catch (Exception ex)
            {
                log.Error("Process Monster File Error:" + ex.Message + " " + ex.InnerException + " " + ex.StackTrace);
            }
        }

    }
}
