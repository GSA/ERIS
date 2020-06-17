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
                //Employee gcimsRecord;
                var columnList = string.Empty;
                var summary = new ERISSummary();
                var fileReader = new FileReader();
                var validate = new ValidateMonster(lookups);
                var em = new EmployeeMapping();
                var save = new SaveData();
                List<string> badRecords;
                var persID = 0;
                var sponsored = 0;

                var monsterAction = "";

                log.Info("Loading Monster File");             
                               
                var MonsterData = fileReader.GetFileData<Employee, EmployeeMapping>(MonsterFile, out badRecords, em);

                //Helpers.AddBadRecordsToSummary(badRecords, ref summary);

                

                ProcessResult updatedResults;

                //Start Processing the Monster Data
                foreach (Employee employeeData in MonsterData)
                {
                    log.Info("Processing Data: " + employeeData.Person.MonsterID);

                    log.Info("Loading GCIMS Data");
                    var allGCIMSData = retrieve.AllGCIMSUpdatedData(employeeData);

                    //If there are critical errors write to the error summary and move to the next record
                    log.Info("Checking for Critical errors for user: " + employeeData.Person.MonsterID);
                    if (Helpers.CheckForErrors(validate, employeeData, summary.UnsuccessfulProcessed, ref log))
                        continue;

                    Helpers.CleanupMonsterData(employeeData);

                    //Looking for matching record.
                    log.Info("Looking for matching record: " + employeeData.Person.MonsterID);
                    ReturnAction action = new ReturnAction();
                    ReturnRecords records = new ReturnRecords();
                    //gcimsRecord = Helpers.RecordFound(employeeData, allGCIMSData, ref log);
                    monsterAction = action.MonsterAction(employeeData);
                    

                    switch (monsterAction)
                    {
                        case "Update Record":
                            if (!Helpers.AreEqualGcimsToHr(allGCIMSData.Single(), employeeData, out columnList, ref log))
                            {
                                log.Info("Update record for user: " + employeeData.Person.MonsterID);
                                persID = records.GetUpdatedID(employeeData);
                                sponsored = records.GetSponsor(employeeData);
                                updatedResults = save.UpdatePersonInformation(employeeData);
                                if (updatedResults.Result > 0)
                                {
                                    summary.UpdatedRecordsProcessed.Add(new UpdatedSummary
                                    {
                                        MonsterID = employeeData.Person.MonsterID,
                                        GCIMSID = persID,
                                        FirstName = employeeData.Person.FirstName,
                                        MiddleName = employeeData.Person.MiddleName,
                                        LastName = employeeData.Person.LastName,
                                        Suffix = employeeData.Person.Suffix,
                                        Sponsored = sponsored,
                                        UpdatedFields = columnList

                                    });
                                }
                            }
                            break;
                            //case "Potential Match":
                            //    log.Info("Flagged record for user: " + employeeData.Person.MonsterID);
                            //    persID = records.GetFlaggedID(employeeData);
                            //    summary.FlaggedRecordsProcessed.Add(new FlaggedSummary
                            //    {
                            //        MonsterID = employeeData.Person.MonsterID,
                            //        GCIMSID = persID,
                            //        FirstName = employeeData.Person.FirstName,
                            //        MiddleName = employeeData.Person.MiddleName,
                            //        LastName = employeeData.Person.LastName,
                            //        Suffix = employeeData.Person.Suffix,
                            //    });
                            //    break;
                            //case "New Record":
                            //    log.Info("Create record for user: " + employeeData.Person.MonsterID);
                            //    persID = save.InsertNewEmployee(employeeData);
                            //    summary.CreatedRecordsProcessed.Add(new CreatedSummary
                            //    {
                            //        MonsterID = employeeData.Person.MonsterID,
                            //        GCIMSID = persID,
                            //        FirstName = employeeData.Person.FirstName,
                            //        MiddleName = employeeData.Person.MiddleName,
                            //        LastName = employeeData.Person.LastName,
                            //        Suffix = employeeData.Person.Suffix
                            //    });
                            //    break;

                    }


                    //if (TerritoriesNotCountriesArray.Contains(employeeData.Birth.CountryOfBirth.ToLower()) && string.IsNullOrWhiteSpace(employeeData.Birth.StateOfBirth))
                    //{
                    //    switch (employeeData.Birth.CountryOfBirth.ToLower())
                    //    {
                    //        case "rq":
                    //            { employeeData.Birth.StateOfBirth = "PR"; }
                    //            break;
                    //        case "gq":
                    //            { employeeData.Birth.StateOfBirth = "GU"; }
                    //            break;
                    //        case "vq":
                    //            { employeeData.Birth.StateOfBirth = "VI"; }
                    //            break;
                    //        case "aq":
                    //            { employeeData.Birth.StateOfBirth = "AS"; }
                    //            break;
                    //        default:
                    //            { employeeData.Birth.StateOfBirth = ""; }
                    //            break;
                    //    }
                    //    employeeData.Birth.CountryOfBirth = "US";
                    //}




                }

                emailData.MonsterFilename = Path.GetFileName(MonsterFile);
                emailData.ItemsProcessed = MonsterData.Count;
                //emailData.CreateRecord = summary.CreatedRecordsProcessed.Count;
                emailData.UpdateRecord = summary.UpdatedRecordsProcessed.Count;
                //emailData.FlagRecord = summary.FlaggedRecordsProcessed.Count;
                emailData.ErrorRecord = summary.UnsuccessfulProcessed.Count;

                //Add log entries
                log.Info("Total records " + String.Format("{0:#,###0}", MonsterData.Count));
                //log.Info("Records created: " + String.Format("{0:#,###0}", summary.CreatedRecordsProcessed.Count));
                log.Info("Records updated: " + String.Format("{0:#,###0}", summary.UpdatedRecordsProcessed.Count));
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
