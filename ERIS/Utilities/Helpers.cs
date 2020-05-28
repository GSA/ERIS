using ERIS.Models;
using ERIS.Process;
using ERIS.Validation;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ERIS.Utilities
{
    internal static class Helpers
    {
        /// <summary>
        /// Hashes SSN
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        public static byte[] HashSsn(string ssn)
        {
            byte[] hashedFullSsn = null;

            SHA256 shaM = new SHA256Managed();

            ssn = ssn.Replace("-", string.Empty);

            using (shaM)
            {
                hashedFullSsn = shaM.ComputeHash(Encoding.UTF8.GetBytes(ssn));
            }

            return hashedFullSsn;
        }

        ///// <summary>
        ///// Determines if 2 same type object are equal. Fields can be ignored
        ///// </summary>
        ///// <param name="gcimsData"></param>
        ///// <param name="monsterData"></param>
        ///// <param name="propertyNameList"></param>
        ///// <param name="log"></param>
        ///// <returns></returns>
        //public static bool AreEqualGcimsToHr(Employee gcimsData, Employee monsterData, out string propertyNameList, ref ILog log)
        //{
        //    var compareLogic = new CompareLogic
        //    {
        //        Config = { TreatStringEmptyAndNullTheSame = true, CaseSensitive = false, MaxDifferences = 100 }
        //    };

        //    compareLogic.Config.CustomComparers.Add(new EmployeeComparer(RootComparerFactory.GetRootComparer()));

        //    var result = compareLogic.Compare(gcimsData, hrData);

        //    var diffs = result.Differences.Select(a => a.PropertyName).ToArray();
        //    var localPropertyNameList = string.Join(",", diffs);
        //    propertyNameList = localPropertyNameList;
        //    if (diffs?.Length > 0)
        //    {
        //        log.Info($"Property differences include: {localPropertyNameList}");
        //    }

        //    return result.AreEqual;
        //}


        /// <summary>
        /// Returns an Employee object if match found in db
        /// </summary>
        /// <param name="employeeData"></param>
        /// <param name="allGcimsData"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static Employee RecordFound(Employee employeeData, List<Employee> allGCIMSData, ref ILog log)
        {
            var MonsterMatch = allGCIMSData.Where(w => employeeData.Person.GCIMSID == w.Person.GCIMSID).ToList();

            if (MonsterMatch.Count > 1)
            {
                log.Info("Multiple Monster IDs Found: " + employeeData.Person.GCIMSID);

                return null;
            }
            else if (MonsterMatch.Count == 1)
            {
                log.Info("Matching record found by emplID: " + employeeData.Person.GCIMSID);

                return MonsterMatch.Single();
            }
            else if (MonsterMatch.Count == 0)
            {
                log.Info("Trying to match record by Lastname, Birth Date and SSN: " + employeeData.Person.GCIMSID);

                var nameMatch = allGCIMSData.Where(w =>
                    employeeData.Person.LastName.ToLower().Trim().Equals(w.Person.LastName.ToLower().Trim()) &&
                    employeeData.Person.SocialSecurityNumber.Equals(w.Person.SocialSecurityNumber) &&
                    employeeData.Birth.DateOfBirth.Equals(w.Birth.DateOfBirth)).ToList();

                if (nameMatch.Count == 0 || nameMatch.Count > 1)
                {
                    log.Info("Match not found by name for user: " + employeeData.Person.GCIMSID);
                    return null;
                }
                else if (nameMatch.Count == 1)
                {
                    log.Info("Match found by name for user: " + employeeData.Person.GCIMSID);
                    return nameMatch.Single();
                }
            }

            return null;
        }

        /// <summary>
        /// Adds a bad record to the summary
        /// </summary>
        /// <param name="badRecords"></param>
        /// <param name="summary"></param>
        public static void AddBadRecordsToSummary(IEnumerable<string> badRecords, ref ERISSummary summary)
        {
            foreach (var item in badRecords)
            {
                var parts = new List<string>();
                var s = item.removeItems(new[] { "\"" });
                parts.AddRange(s.Split('~'));
                var obj = new ProcessedSummary
                {
                    Action = "Invalid Record From CSV File",
                    MonsterID = -1,
                    LastName = parts.Count > 1 ? parts[1] : "Unknown Last Name",
                    FirstName = parts.Count > 3 ? parts[3] : "Unknown First Name",
                    MiddleName = parts.Count > 4 ? parts[4] : "Unknown Middle Name"
                };
                summary.UnsuccessfulUsersProcessed.Add(obj);
            }
        }

        /// <summary>
        /// Processes the validation and returns if a record has validation errors
        /// </summary>
        /// <param name="validate"></param>
        /// <param name="employeeData"></param>
        /// <param name="FlaggedProcessed"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static bool CheckForErrors(ValidateMonster validate, Employee employeeData, List<ProcessedSummary> FlaggedProcessed, ref ILog log)
        {
            var validationHelper = new ValidationHelper();
            //var criticalErrors = validate.ValidateEmployeeCriticalInfo(employeeData);

            //if (criticalErrors.IsValid) return false;
            //log.Warn("Errors found for user: " + employeeData.Person.MonsterID + "(" + criticalErrors.Errors.Count + ")");

            FlaggedProcessed.Add(new ProcessedSummary
            {
                MonsterID = -1,
                FirstName = employeeData.Person.FirstName,
                MiddleName = employeeData.Person.MiddleName,
                LastName = employeeData.Person.LastName,
                SocialSecurityNumber = employeeData.Person.SocialSecurityNumber,
                DateOfBirth = employeeData.Birth.DateOfBirth
            });

            return true;

        }

        /// <summary>
        /// Formats phone numbers to be in correct format to insert into db
        /// </summary>
        /// <param name="employeeData"></param>
        public static void CleanupMonsterData(Employee employeeData)
        {
            //Address clean up
            CleanAddress(employeeData.Address);

            //Phone clean up
            employeeData.Phone.HomePhone = employeeData.Phone.HomePhone.RemovePhoneFormatting();
            employeeData.Phone.WorkCell = employeeData.Phone.WorkCell.RemovePhoneFormatting();
            employeeData.Phone.PersonalCell= employeeData.Phone.PersonalCell.RemovePhoneFormatting();;

        }

        /// <summary>
        /// Removes pound signs from the 3 personal addresses
        /// </summary>
        /// <param name="a"></param>
        private static void CleanAddress(Address a)
        {
            var s = new[] { "#" };
            a.HomeAddress1 = a.HomeAddress1.removeItems(s);
            a.HomeAddress2 = a.HomeAddress2.removeItems(s);
            a.HomeAddress3 = a.HomeAddress3.removeItems(s);
            a.HomeCity = a.HomeCity.removeItems(s);
        }

    }
}
