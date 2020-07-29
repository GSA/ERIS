using ERIS.Models;
using ERIS.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERIS.Data
{
    internal class SaveData
    {
        //Reference to logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Set up connection
        private readonly MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ERIS"].ToString());

        private readonly MySqlCommand cmd = new MySqlCommand();

        public SaveData()
        {

        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="saveData"></param>
        /// <returns></returns>
        /// Change to person data
        public ProcessResult UpdatePersonInformation(Int64 persID, Employee monsterData)
        {
            try
            {
                using (conn)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ERIS_UpdatePerson";

                        cmd.Parameters.Clear();

                        MySqlParameter[] personParameters = new MySqlParameter[]
                        {
                            new MySqlParameter { ParameterName = "monsterID", Value = monsterData.Person.MonsterID, MySqlDbType = MySqlDbType.VarChar, Size = 90},
                            new MySqlParameter { ParameterName = "persID", Value = persID, MySqlDbType = MySqlDbType.Int64},
                            new MySqlParameter { ParameterName = "firstname", Value = monsterData.Person.FirstName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                            new MySqlParameter { ParameterName = "middlename", Value = monsterData.Person.MiddleName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                            new MySqlParameter { ParameterName = "lastname", Value = monsterData.Person.LastName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                            new MySqlParameter { ParameterName = "suffix", Value = monsterData.Person.Suffix, MySqlDbType = MySqlDbType.VarChar, Size = 12},
                            new MySqlParameter { ParameterName = "ssn", Value = monsterData.Person.SocialSecurityNumber, MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "dob", Value = monsterData.Birth.DateOfBirth, MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "cityOfBirth", Value = monsterData.Birth.CityOfBirth, MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "stateOfBirth", Value = monsterData.Birth.StateOfBirth, MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "countryOfBirth", Value = monsterData.Birth.CountryOfBirth, MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "countryOfCitizenship", Value = monsterData.Birth.CountryOfCitizenship, MySqlDbType = MySqlDbType.VarChar, Size = 2},
                            new MySqlParameter { ParameterName = "isCitizen", Value = monsterData.Birth.Citizen, MySqlDbType = MySqlDbType.Byte},
                            new MySqlParameter { ParameterName = "homeAddress1", Value = monsterData.Address.HomeAddress1, MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "homeAddress2", Value = monsterData.Address.HomeAddress2, MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "homeAddress3", Value = monsterData.Address.HomeAddress3, MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "homeCity", Value = monsterData.Address.HomeCity, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                            new MySqlParameter { ParameterName = "homeState", Value = monsterData.Address.HomeState, MySqlDbType = MySqlDbType.VarChar, Size = 2},
                            new MySqlParameter { ParameterName = "homeZipCode", Value = monsterData.Address.HomeZipCode, MySqlDbType = MySqlDbType.VarChar, Size = 5},
                            new MySqlParameter { ParameterName = "homeCountry", Value = monsterData.Address.HomeCountry, MySqlDbType = MySqlDbType.VarChar, Size = 2},
                            new MySqlParameter { ParameterName = "gender", Value = monsterData.Person.Gender, MySqlDbType = MySqlDbType.VarChar, Size = 1},
                            new MySqlParameter { ParameterName = "jobTitle", Value = monsterData.Position.JobTitle, MySqlDbType = MySqlDbType.VarChar, Size = 70},
                            new MySqlParameter { ParameterName = "persRegion", Value = monsterData.Position.Region, MySqlDbType = MySqlDbType.VarChar, Size = 3},
                            new MySqlParameter { ParameterName = "majorOrg", Value = monsterData.Position.MajorOrg, MySqlDbType = MySqlDbType.VarChar, Size = 1},
                            new MySqlParameter { ParameterName = "officeSymbol", Value = monsterData.Position.OfficeSymbol, MySqlDbType = MySqlDbType.VarChar, Size = 18},
                            new MySqlParameter { ParameterName = "homePhone", Value = monsterData.Phone.HomePhone, MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "homeCell", Value = monsterData.Phone.PersonalCell, MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "homeEmail", Value = monsterData.Person.HomeEmail, MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "isVirtual", Value = monsterData.Position.IsVirtual, MySqlDbType = MySqlDbType.Byte},
                            new MySqlParameter { ParameterName = "virtualRegion", Value = monsterData.Position.VirtualRegion, MySqlDbType = MySqlDbType.VarChar, Size = 3},
                            new MySqlParameter { ParameterName = "workBuilding", Value = monsterData.Building.BuildingLocationCode, MySqlDbType = MySqlDbType.VarChar, Size = 6},
                            new MySqlParameter { ParameterName = "hrEmail", Value = monsterData.Person.HREmail, MySqlDbType = MySqlDbType.VarChar, Size = 64},
                            new MySqlParameter { ParameterName = "result", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output},
                            new MySqlParameter { ParameterName = "actionMsg", MySqlDbType = MySqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output },
                            new MySqlParameter { ParameterName = "SQLExceptionWarning", MySqlDbType=MySqlDbType.VarChar, Size=4000, Direction = ParameterDirection.Output },
                        };

                        cmd.Parameters.AddRange(personParameters);

                        cmd.ExecuteNonQuery();

                        return new ProcessResult
                        {
                            Result = (int)cmd.Parameters["result"].Value,
                            Action = cmd.Parameters["actionMsg"].Value.ToString(),
                            Error = cmd.Parameters["SQLExceptionWarning"].Value.ToString()
                        };
                    }
                }
            }
            //Catch all errors
            catch (Exception ex)
            {
                log.Error("Updating GCIMS Record: " + ex.Message + " - " + ex.InnerException);
                return new ProcessResult
                {
                    Result = -1,
                    Action = "-1",
                    Error = ex.Message.ToString()
                };
            }
        }

        /// <summary>
        /// Function that calls stored procedure for inserting a user
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="storedProcedure"></param>
        /// <returns>ID of new user</returns>
        public int InsertNewEmployee(Employee monsterData)
        {
            try
            {
                using (conn)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ERIS_InsertNewEmployee";

                        cmd.Parameters.Clear();

                        string persGuid = System.Guid.NewGuid().ToString();

                        MySqlParameter[] UserParamters = new MySqlParameter[]
                            {

                                new MySqlParameter { ParameterName = "monsterID", Value = monsterData.Person.MonsterID, MySqlDbType = MySqlDbType.VarChar, Size = 90},
                                new MySqlParameter { ParameterName = "firstName", Value = monsterData.Person.FirstName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                                new MySqlParameter { ParameterName = "middleName", Value = monsterData.Person.MiddleName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                                new MySqlParameter { ParameterName = "lastName", Value = monsterData.Person.LastName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                                new MySqlParameter { ParameterName = "suffix", Value = monsterData.Person.Suffix, MySqlDbType = MySqlDbType.VarChar, Size = 3},
                                new MySqlParameter { ParameterName = "SSN", Value = monsterData.Person.SocialSecurityNumber, MySqlDbType = MySqlDbType.TinyBlob },
                                new MySqlParameter { ParameterName = "HashedSSN", Value = Helpers.HashSsn(monsterData.Person.SocialSecurityNumber), MySqlDbType = MySqlDbType.Binary, Size = 32 },
                                new MySqlParameter { ParameterName = "HashedSSNLast4", Value = Helpers.HashSsn(monsterData.Person.SocialSecurityNumber.Substring(5,4)), MySqlDbType = MySqlDbType.Binary, Size = 32 },
                                new MySqlParameter { ParameterName = "cityOfBirth", Value = monsterData.Birth.CityOfBirth, MySqlDbType = MySqlDbType.TinyBlob},
                                new MySqlParameter { ParameterName = "stateOfBirth", Value = monsterData.Birth.StateOfBirth, MySqlDbType = MySqlDbType.TinyBlob},
                                new MySqlParameter { ParameterName = "countryOfBirth", Value = monsterData.Birth.CountryOfBirth, MySqlDbType = MySqlDbType.TinyBlob},
                                new MySqlParameter { ParameterName = "countryOfCitizenship", Value = monsterData.Birth.CountryOfCitizenship, MySqlDbType = MySqlDbType.VarChar, Size = 2},
                                new MySqlParameter { ParameterName = "isCitizen", Value = monsterData.Birth.Citizen, MySqlDbType = MySqlDbType.Byte},
                                new MySqlParameter { ParameterName = "homeAddress1", Value = monsterData.Address.HomeAddress1, MySqlDbType = MySqlDbType.TinyBlob},
                                new MySqlParameter { ParameterName = "homeAddress2", Value = monsterData.Address.HomeAddress2, MySqlDbType = MySqlDbType.TinyBlob},
                                new MySqlParameter { ParameterName = "homeAddress3", Value = monsterData.Address.HomeAddress3, MySqlDbType = MySqlDbType.TinyBlob},
                                new MySqlParameter { ParameterName = "homeCity", Value = monsterData.Address.HomeCity, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                                new MySqlParameter { ParameterName = "homeState", Value = monsterData.Address.HomeState, MySqlDbType = MySqlDbType.VarChar, Size = 2},
                                new MySqlParameter { ParameterName = "homeZipCode", Value = monsterData.Address.HomeZipCode, MySqlDbType = MySqlDbType.VarChar, Size = 5},
                                new MySqlParameter { ParameterName = "homeCountry", Value = monsterData.Address.HomeCountry, MySqlDbType = MySqlDbType.VarChar, Size = 2},
                                new MySqlParameter { ParameterName = "dateOfBirth", Value = monsterData.Birth.DateOfBirth, MySqlDbType = MySqlDbType.TinyBlob},
                                new MySqlParameter { ParameterName = "gender", Value = monsterData.Person.Gender, MySqlDbType = MySqlDbType.VarChar, Size = 1},
                                new MySqlParameter { ParameterName = "jobTitle", Value = monsterData.Position.JobTitle, MySqlDbType = MySqlDbType.VarChar, Size = 70},
                                new MySqlParameter { ParameterName = "persRegion", Value = monsterData.Position.Region, MySqlDbType = MySqlDbType.VarChar, Size = 3},
                                new MySqlParameter { ParameterName = "majorOrg", Value = monsterData.Position.MajorOrg, MySqlDbType = MySqlDbType.VarChar, Size = 1},
                                new MySqlParameter { ParameterName = "officeSymbol", Value = monsterData.Position.OfficeSymbol, MySqlDbType = MySqlDbType.VarChar, Size = 18},
                                new MySqlParameter { ParameterName = "homePhone", Value = monsterData.Phone.HomePhone, MySqlDbType = MySqlDbType.TinyBlob},
                                new MySqlParameter { ParameterName = "homeCell", Value = monsterData.Phone.PersonalCell, MySqlDbType = MySqlDbType.TinyBlob},
                                new MySqlParameter { ParameterName = "homeEmail", Value = monsterData.Person.HomeEmail, MySqlDbType = MySqlDbType.TinyBlob},
                                new MySqlParameter { ParameterName = "workBuilding", Value = monsterData.Building.BuildingLocationCode, MySqlDbType = MySqlDbType.VarChar, Size = 6},
                                new MySqlParameter { ParameterName = "isVirtual", Value = monsterData.Position.IsVirtual, MySqlDbType = MySqlDbType.Byte},
                                new MySqlParameter { ParameterName = "virtualRegion", Value = monsterData.Position.VirtualRegion, MySqlDbType = MySqlDbType.VarChar, Size = 3},
                                new MySqlParameter { ParameterName = "hrEmail", Value = monsterData.Person.HREmail, MySqlDbType = MySqlDbType.VarChar, Size = 64},
                                new MySqlParameter { ParameterName = "PersGUID", Value = persGuid, MySqlDbType=MySqlDbType.VarChar, Size=36},
                                new MySqlParameter { ParameterName = "PersID", MySqlDbType=MySqlDbType.Int32, Direction = ParameterDirection.Output },
                                new MySqlParameter { ParameterName = "result", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output},
                                new MySqlParameter { ParameterName = "actionMsg", MySqlDbType = MySqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output },
                                new MySqlParameter { ParameterName = "SQLExceptionWarning", MySqlDbType=MySqlDbType.VarChar, Size=4000, Direction = ParameterDirection.Output },

                            };

                        cmd.Parameters.AddRange(UserParamters);

                        cmd.ExecuteNonQuery();

                        log.Info(String.Format("InsertNewUser completed with persId:{0} and SqlException:{1}", cmd.Parameters["PersID"].Value, cmd.Parameters["SQLExceptionWarning"].Value));
                    }
                }
            }

            catch (Exception ex)
            {
                log.Error("Add New Employee: " + ex.Message + " - " + ex.InnerException);

            }
            //Returns the Person ID
            return (int)cmd.Parameters["PersID"].Value;
        }
    }
}
