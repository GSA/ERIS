using AutoMapper;
using ERIS.Lookups;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace ERIS.Data
{
    internal class LoadLookupData
    {
        //Reference to logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ERIS"].ToString());
        private readonly MySqlCommand cmd = new MySqlCommand();

        private readonly IMapper lookupMapper;

        public LoadLookupData(IMapper mapper)
        {
            lookupMapper = mapper;

            lookupMapper.ConfigurationProvider.CompileMappings();
        }

        public Lookup GetEmployeeLookupData()
        {
            Lookup lookups = new Lookup();

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
                        cmd.CommandText = "ERIS_Get_Employee_Lookups";

                        MySqlDataReader lookupData = cmd.ExecuteReader();

                        using (lookupData)
                        {
                            if (lookupData.HasRows)
                                lookups = MapEmployeeLookupData(lookupData);
                        }
                    }
                }

                return lookups;
            }
            catch (Exception ex)
            {
                log.Error("Something went wrong" + " - " + ex.Message + " - " + ex.InnerException);

                return lookups;
            }
        }

        private Lookup MapEmployeeLookupData(MySqlDataReader lookupData)
        {
            Lookup lookup = new Lookup();

            //lookup_country
            lookup.countryLookup = lookupMapper.Map<IDataReader, List<CountryLookup>>(lookupData);

            //lookup_state
            lookupData.NextResult();
            lookup.usStateLookup = lookupMapper.Map<IDataReader, List<StateLookup>>(lookupData);

            lookupData.NextResult();
            lookup.mxStateLookup = lookupMapper.Map<IDataReader, List<StateLookup>>(lookupData);

            lookupData.NextResult();
            lookup.caStateLookup = lookupMapper.Map<IDataReader, List<StateLookup>>(lookupData);

            //lookup_region
            lookupData.NextResult();
            lookup.regionLookup = lookupMapper.Map<IDataReader, List<RegionLookup>>(lookupData);

            //lookup_building
            lookupData.NextResult();
            lookup.BuildingLookup = lookupMapper.Map<IDataReader, List<BuildingLookup>>(lookupData);

            //lookup_workEmail
            lookupData.NextResult();
            lookup.EmailLookup = lookupMapper.Map<IDataReader, List<EmailLookup>>(lookupData);

            return lookup;
        }
    }
}