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
    internal class ReturnAction
    {
        //Set up connection
        private readonly MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ERIS"].ToString());

        private readonly MySqlCommand cmd = new MySqlCommand();

        /// <summary>
        /// Calls stored procedure that checks if it is the new record, potential match or existing record need to be updated 
        /// </summary>
        /// <param name="ssn"></param>
        /// <param name="dob"></param>
        /// <param name="firstname"></param>
        /// /// <param name="middlename"></param>
        /// /// <param name="lastname"></param>
        /// /// <param name="suffix"></param>
        /// <returns></returns>
        public string MonsterAction(string ssn, string dob, string firstname, string middlename, string lastname, string suffix)
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
                        cmd.CommandText = "ERIS_Definer";

                        cmd.Parameters.Clear();

                        MySqlParameter[] erisParamaters = new MySqlParameter[]
                        {
                            new MySqlParameter { ParameterName = "firstname", Value = firstname, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                            new MySqlParameter { ParameterName = "middlename", Value = firstname, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                            new MySqlParameter { ParameterName = "lastname", Value = lastname, MySqlDbType = MySqlDbType.VarChar, Size = 60},
                            new MySqlParameter { ParameterName = "suffix", Value = suffix == "" ? DBNull.Value.ToString() : suffix, MySqlDbType = MySqlDbType.VarChar, Size = 12},
                            new MySqlParameter { ParameterName = "ssn", Value = Helpers.HashSsn(ssn), MySqlDbType = MySqlDbType.VarBinary, Size = 32},
                            new MySqlParameter { ParameterName = "dob", Value = dob, MySqlDbType = MySqlDbType.VarChar, Size = 11},
                            new MySqlParameter { ParameterName = "monsterAction", MySqlDbType=MySqlDbType.VarChar, Size=20, Direction = ParameterDirection.Output },
                        };

                        cmd.Parameters.AddRange(erisParamaters);

                        cmd.ExecuteNonQuery();

                        return cmd.Parameters["monsterAction"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

    }
}
