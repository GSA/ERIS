using ERIS.Models;
using ERIS.Process;
using ERIS.Validation;
using KellermanSoftware.CompareNetObjects;
using log4net;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

        /// <summary>
        /// Determines if 2 same type object are equal. Fields can be ignored
        /// </summary>
        /// <param name="gcimsData"></param>
        /// <param name="erisData"></param>
        /// <param name="propertyNameList"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static bool AreEqualGcimsToMonster1(Employee gcimsData, Employee erisData, out string propertyNameList, ref ILog log)
        {
            var compareLogic = new CompareLogic
            {
                Config = { TreatStringEmptyAndNullTheSame = true, CaseSensitive = false, MaxDifferences = 100 }
            };

            compareLogic.Config.CustomComparers.Add(new EmployeeComparerforUpdated(RootComparerFactory.GetRootComparer()));

            compareLogic.Config.CustomComparers.Add(new EmployeeComparerforFlagged(RootComparerFactory.GetRootComparer()));

            var result = compareLogic.Compare(gcimsData, erisData);

            var diffs = result.Differences.Select(a => a.PropertyName).ToArray();

            var localPropertyNameList = string.Join(",", diffs);
            propertyNameList = localPropertyNameList;
            if (diffs?.Length > 0)
            {
                log.Info($"Property differences include: {localPropertyNameList}");                
            }

            return result.AreEqual;

        }

        /// <summary>
        /// Determines if 2 same type object are equal. Fields can be ignored
        /// </summary>
        /// <param name="gcimsData"></param>
        /// <param name="erisData"></param>
        /// <param name="propertyNameList"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static bool AreEqualGcimsToMonster2(Employee gcimsData, Employee erisData, out string propertyNameList, ref ILog log)
        {
            var compareLogic = new CompareLogic
            {
                Config = { TreatStringEmptyAndNullTheSame = true, CaseSensitive = false, MaxDifferences = 100 }
            };

            compareLogic.Config.CustomComparers.Add(new EmployeeComparerforFlagged(RootComparerFactory.GetRootComparer()));

            var result = compareLogic.Compare(gcimsData, erisData);

            var matches = result.Differences.Select(a => a.PropertyName).ToArray();

            var localPropertyNameList = string.Join(",", matches);
            propertyNameList = localPropertyNameList;

            if (matches?.Length > 0)
            {
                log.Info($"Property matching fields include: {localPropertyNameList}");

            }
            return result.AreEqual;

        }


        /// <summary>
        /// Processes the validation and returns if a record has validation errors
        /// </summary>
        /// <param name="validate"></param>
        /// <param name="employeeData"></param>
        /// <param name="FlaggedProcessed"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static bool CheckForErrors(ValidateMonster validate, Employee employeeData, List<ErrorSummary> UnsuccessfulProcessed, ref ILog log)
        {
            var validationHelper = new ValidationHelper();
            var criticalErrors = validate.ValidateEmployeeCriticalInfo(employeeData);

            if (criticalErrors.IsValid) return false;
            log.Warn("Errors found for user: " + employeeData.Person.MonsterID + "(" + criticalErrors.Errors.Count + ")");

            UnsuccessfulProcessed.Add(new ErrorSummary
            {
                MonsterID = employeeData.Person.MonsterID,
                FirstName = employeeData.Person.FirstName,
                MiddleName = employeeData.Person.MiddleName,
                LastName = employeeData.Person.LastName,
                Suffix = employeeData.Person.Suffix,
                ValidationErrors = validationHelper.GetErrors(criticalErrors.Errors, ValidationHelper.Monster.Monsterfile).TrimEnd(',')
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
            //employeeData.Phone.WorkCell = employeeData.Phone.WorkCell.RemovePhoneFormatting();
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
