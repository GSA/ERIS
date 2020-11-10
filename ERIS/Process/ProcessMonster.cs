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
        private ReviewSummary reviewsummary = new ReviewSummary();
        private ImportSummary importsummary = new ImportSummary();
        private UpdateSummary updatesummary = new UpdateSummary();

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
                var columnList = string.Empty;
                var summary = new ERISSummary();
                var fileReader = new FileReader();
                var validate = new ValidateMonster(lookups);
                var em = new EmployeeMapping();
                var save = new SaveData();
                List<string> badRecords;
                var persID = 0;
                var sponsorship = "";

                var monsterAction = "";

                log.Info("Loading Monster File");             
                               
                var MonsterData = fileReader.GetFileData<Employee, EmployeeMapping>(MonsterFile, out badRecords, em);

                ProcessResult updatedResults;

                //Start Processing the Monster Data
                foreach (Employee employeeData in MonsterData)
                {
                    log.Info("Processing Data: " + employeeData.Person.MonsterID);

                    //If there are critical errors write to the error summary and move to the next record
                    log.Info("Checking for Critical errors for user: " + employeeData.Person.MonsterID);
                    if (Helpers.CheckForErrors(validate, employeeData, summary.UnsuccessfulProcessed, ref log))
                        continue;

                    //Helpers.CleanupMonsterData(employeeData);

                    log.Info("Loading GCIMS Data");
                    var allGCIMSUpdatedData = retrieve.AllGCIMSUpdatedData(employeeData);
                    var allGCIMSFlaggedData = retrieve.AllGCIMSFlaggeddData(employeeData);

                    employeeData.Person.Name = employeeData.Person.FirstName + " " + employeeData.Person.MiddleName + " " + employeeData.Person.LastName + " " + employeeData.Person.Suffix;

                    //Looking for matching record.
                    log.Info("Looking for matching record: " + employeeData.Person.MonsterID);
                    ReturnAction action = new ReturnAction();
                    ReturnRecords records = new ReturnRecords();
                    monsterAction = action.MonsterAction(employeeData);
                    

                    switch (monsterAction)
                    {
                        case "Update Record":
                            if (!Helpers.AreEqualGcimsToMonster1(allGCIMSUpdatedData.FirstOrDefault(), employeeData, out columnList, ref log))
                            {
                                log.Info("Update record for user: " + employeeData.Person.MonsterID);
                                persID = records.GetUpdatedID(employeeData);
                                sponsorship = records.GetSponsor(employeeData);
                                updatedResults = save.UpdatePersonInformation(persID, employeeData);
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
                                        Sponsorship = sponsorship,
                                        UpdatedFields = columnList

                                    });
                                }

                            }
                            else
                            {
                                log.Info("Update record for user: " + employeeData.Person.MonsterID);
                                persID = records.GetUpdatedID(employeeData);
                                sponsorship = records.GetSponsor(employeeData);
                                updatedResults = save.UpdatePersonInformation(persID, employeeData);
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
                                        Sponsorship = sponsorship,
                                        UpdatedFields = columnList

                                    });
                                }

                            }
                            break;
                        case "Potential Match":
                            if (!Helpers.AreEqualGcimsToMonster2(allGCIMSFlaggedData.FirstOrDefault(), employeeData, out columnList, ref log))
                            {
                                log.Info("Flagged record for user: " + employeeData.Person.MonsterID);
                                //persID = records.GetFlaggedID(employeeData);
                                summary.FlaggedRecordsProcessed.Add(new FlaggedSummary
                                {
                                    MonsterID = employeeData.Person.MonsterID,
                                    //GCIMSID = persID,
                                    FirstName = employeeData.Person.FirstName,
                                    MiddleName = employeeData.Person.MiddleName,
                                    LastName = employeeData.Person.LastName,
                                    Suffix = employeeData.Person.Suffix,
                                    MatchingFields = columnList,
                                    HREmail = employeeData.Person.HREmail
                                });

                            }
                            break;
                        case "New Record":
                            log.Info("Create record for user: " + employeeData.Person.MonsterID);
                            persID = save.InsertNewEmployee(employeeData);
                            summary.CreatedRecordsProcessed.Add(new CreatedSummary
                            {
                                MonsterID = employeeData.Person.MonsterID,
                                GCIMSID = persID,
                                FirstName = employeeData.Person.FirstName,
                                MiddleName = employeeData.Person.MiddleName,
                                LastName = employeeData.Person.LastName,
                                Suffix = employeeData.Person.Suffix
                            });
                            break;

                    }

                }

                emailData.MonsterFilename = Path.GetFileName(MonsterFile);
                emailData.ItemsProcessed = MonsterData.Count;
                emailData.CreateRecord = summary.CreatedRecordsProcessed.Count;
                emailData.UpdateRecord = summary.UpdatedRecordsProcessed.Count;
                emailData.FlagRecord = summary.FlaggedRecordsProcessed.Count;
                emailData.ErrorRecord = summary.UnsuccessfulProcessed.Count;

                for (int i = 0; i < emailData.FlagRecord; i++)
                {
                    reviewsummary.SendReviewSummaryEMail(summary.FlaggedRecordsProcessed[i]);
                }

                for (int i = 0; i < emailData.CreateRecord; i++)
                {
                    importsummary.SendImportSummaryEMail(summary.CreatedRecordsProcessed[i]);
                }

                for (int i = 0; i < emailData.UpdateRecord; i++)
                {
                    updatesummary.SendUpdateSummaryEMail(summary.UpdatedRecordsProcessed[i]);
                }

                //Add log entries
                log.Info("Total records " + String.Format("{0:#,###0}", MonsterData.Count));
                log.Info("Records created: " + String.Format("{0:#,###0}", summary.CreatedRecordsProcessed.Count));
                log.Info("Records updated: " + String.Format("{0:#,###0}", summary.UpdatedRecordsProcessed.Count));
                log.Info("Records flagged: " + String.Format("{0:#,###0}", summary.FlaggedRecordsProcessed.Count));
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
