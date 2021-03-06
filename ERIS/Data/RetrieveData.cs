﻿using AutoMapper;
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
    class RetrieveData
    {
        //Reference to logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IMapper retrieveMapper;

        public RetrieveData(IMapper mapper)
        {
            retrieveMapper = mapper;

            retrieveMapper.ConfigurationProvider.CompileMappings();
        }

        public List<Employee> AllGCIMSUpdatedData(Employee monsterData)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ERIS"].ToString());

                MySqlCommand cmd = new MySqlCommand();

                List<Employee> allGCIMSData = new List<Employee>();

                using (conn)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    using (cmd)
                    {
                        MySqlDataReader gcimsData;

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ERIS_GetAllUpdatedRecords";
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("firstname", monsterData.Person.FirstName);
                        cmd.Parameters.AddWithValue("middlename", monsterData.Person.MiddleName);
                        cmd.Parameters.AddWithValue("lastname", monsterData.Person.LastName);
                        cmd.Parameters.AddWithValue("dob", monsterData.Birth.DateOfBirth);
                        cmd.Parameters.AddWithValue("ssn", monsterData.Person.SocialSecurityNumber);
                        cmd.Parameters.AddWithValue("suffix", monsterData.Person.Suffix);

                        gcimsData = cmd.ExecuteReader();

                        using (gcimsData)
                        {
                            if (gcimsData.HasRows)
                            {
                                allGCIMSData = MapAllGCIMSData(gcimsData);
                            }
                        }
                    }
                }

                return allGCIMSData;
            }
            catch (Exception ex)
            {
                log.Error("GetGCIMSRecord: " + " - " + ex.Message + " - " + ex.InnerException);
                return new List<Employee>();
            }
        }

        public List<Employee> AllGCIMSFlaggeddData(Employee monsterData)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ERIS"].ToString());

                MySqlCommand cmd = new MySqlCommand();

                List<Employee> allGCIMSData = new List<Employee>();

                using (conn)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    using (cmd)
                    {
                        MySqlDataReader gcimsData;

                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ERIS_GetAllFlaggedRecords";
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("firstname", monsterData.Person.FirstName);
                        cmd.Parameters.AddWithValue("middlename", monsterData.Person.MiddleName);
                        cmd.Parameters.AddWithValue("lastname", monsterData.Person.LastName);
                        cmd.Parameters.AddWithValue("dob", monsterData.Birth.DateOfBirth);
                        cmd.Parameters.AddWithValue("ssn", monsterData.Person.SocialSecurityNumber);
                        cmd.Parameters.AddWithValue("suffix", monsterData.Person.Suffix);

                        gcimsData = cmd.ExecuteReader();

                        using (gcimsData)
                        {
                            if (gcimsData.HasRows)
                            {
                                allGCIMSData = MapAllGCIMSData(gcimsData);
                            }
                        }
                    }
                }

                return allGCIMSData;
            }
            catch (Exception ex)
            {
                log.Error("GetGCIMSRecord: " + " - " + ex.Message + " - " + ex.InnerException);
                return new List<Employee>();
            }
        }

        private List<Employee> MapAllGCIMSData(MySqlDataReader gcimsData)
        {
            List<Employee> allRecords = new List<Employee>();

            while (gcimsData.Read())
            {
                Employee employee = new Employee();

                employee.Address = retrieveMapper.Map<IDataReader, Address>(gcimsData);
                employee.Building = retrieveMapper.Map<IDataReader, Building>(gcimsData);
                employee.Birth = retrieveMapper.Map<IDataReader, Birth>(gcimsData);
                employee.Person = retrieveMapper.Map<IDataReader, Person>(gcimsData);
                employee.Phone = retrieveMapper.Map<IDataReader, Phone>(gcimsData);
                employee.Position = retrieveMapper.Map<IDataReader, Position>(gcimsData); //Need to fix SupervisorID

                allRecords.Add(employee);
            }

            return allRecords;
        }
    }

}
