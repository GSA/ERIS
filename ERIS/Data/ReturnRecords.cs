using ERIS.Models;
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
    class ReturnRecords
    {
        //Reference to logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Set up connection
        private readonly MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ERIS"].ToString());

        private readonly MySqlCommand cmd = new MySqlCommand();

        /// <summary>
        /// Calls stored procedure that retrieve pers_id
        /// </summary>
        /// <param name="ssn"></param>
        /// <param name="dob"></param>
        /// <param name="firstname"></param>
        /// /// <param name="middlename"></param>
        /// /// <param name="lastname"></param>
        /// /// <param name="suffix"></param>
        /// <returns></returns>
        public int GetUpdatedID(Employee monsterData)
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
                        cmd.CommandText = "ERIS_UpdatedIDs";

                        cmd.Parameters.Clear();

                        MySqlParameter[] erisParamaters = new MySqlParameter[]
                        {
                            new MySqlParameter { ParameterName = "firstname", Value = monsterData.Person.FirstName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                            new MySqlParameter { ParameterName = "middlename", Value = monsterData.Person.MiddleName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                            new MySqlParameter { ParameterName = "lastname", Value = monsterData.Person.LastName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                            new MySqlParameter { ParameterName = "suffix", Value = monsterData.Person.Suffix, MySqlDbType = MySqlDbType.VarChar, Size = 12},
                            new MySqlParameter { ParameterName = "ssn", Value = monsterData.Person.SocialSecurityNumber, MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "dob", Value = monsterData.Birth.DateOfBirth?.ToString("yyyy-MM-dd"), MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "persID", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output },
                        };

                        cmd.Parameters.AddRange(erisParamaters);

                        cmd.ExecuteNonQuery();

                       
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Get PersID: " + ex.Message + " - " + ex.InnerException);

            }

            return (int)cmd.Parameters["persID"].Value;
        }


        ///// <summary>
        ///// Calls stored procedure that retrieve pers_id
        ///// </summary>
        ///// <param name="ssn"></param>
        ///// <param name="dob"></param>
        ///// <param name="firstname"></param>
        ///// /// <param name="middlename"></param>
        ///// /// <param name="lastname"></param>
        ///// /// <param name="suffix"></param>
        ///// <returns></returns>
        //public int GetFlaggedID(Employee monsterData)
        //{
        //    try
        //    {
        //        using (conn)
        //        {
        //            if (conn.State == ConnectionState.Closed)
        //                conn.Open();

        //            using (cmd)
        //            {
        //                cmd.Connection = conn;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.CommandText = "ERIS_FlaggedIDs";

        //                cmd.Parameters.Clear();

        //                MySqlParameter[] erisParamaters = new MySqlParameter[]
        //                {
        //                    new MySqlParameter { ParameterName = "firstname", Value = monsterData.Person.FirstName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
        //                    new MySqlParameter { ParameterName = "middlename", Value = monsterData.Person.MiddleName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
        //                    new MySqlParameter { ParameterName = "lastname", Value = monsterData.Person.LastName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
        //                    new MySqlParameter { ParameterName = "suffix", Value = monsterData.Person.Suffix, MySqlDbType = MySqlDbType.VarChar, Size = 12},
        //                    new MySqlParameter { ParameterName = "ssn", Value = monsterData.Person.SocialSecurityNumber, MySqlDbType = MySqlDbType.TinyBlob},
        //                    new MySqlParameter { ParameterName = "dob", Value = monsterData.Birth.DateOfBirth?.ToString("yyyy-MM-dd"), MySqlDbType = MySqlDbType.TinyBlob},
        //                    new MySqlParameter { ParameterName = "persID", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output },
        //                };

        //                cmd.Parameters.AddRange(erisParamaters);

        //                cmd.ExecuteNonQuery();


        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Get PersID: " + ex.Message + " - " + ex.InnerException);

        //    }

        //    return (int)cmd.Parameters["persID"].Value;
        //}

        /// <summary>
        /// Calls stored procedure that retrieve pers_mso_sponsored
        /// </summary>
        /// <param name="ssn"></param>
        /// <param name="dob"></param>
        /// <param name="firstname"></param>
        /// /// <param name="middlename"></param>
        /// /// <param name="lastname"></param>
        /// /// <param name="suffix"></param>
        /// <returns></returns>
        public int GetSponsor(Employee monsterData)
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
                        cmd.CommandText = "ERIS_GetSponsorInfo";

                        cmd.Parameters.Clear();

                        MySqlParameter[] erisParamaters = new MySqlParameter[]
                        {
                            new MySqlParameter { ParameterName = "firstname", Value = monsterData.Person.FirstName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                            new MySqlParameter { ParameterName = "middlename", Value = monsterData.Person.MiddleName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                            new MySqlParameter { ParameterName = "lastname", Value = monsterData.Person.LastName, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                            new MySqlParameter { ParameterName = "suffix", Value = monsterData.Person.Suffix, MySqlDbType = MySqlDbType.VarChar, Size = 12},
                            new MySqlParameter { ParameterName = "ssn", Value = monsterData.Person.SocialSecurityNumber, MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "dob", Value = monsterData.Birth.DateOfBirth?.ToString("yyyy-MM-dd"), MySqlDbType = MySqlDbType.TinyBlob},
                            new MySqlParameter { ParameterName = "sponsored", MySqlDbType = MySqlDbType.Int32, Direction = ParameterDirection.Output },
                        };

                        cmd.Parameters.AddRange(erisParamaters);

                        cmd.ExecuteNonQuery();


                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Get Sponsor Info: " + ex.Message + " - " + ex.InnerException);

            }

            return (int)cmd.Parameters["sponsored"].Value;
        }
    }
}
