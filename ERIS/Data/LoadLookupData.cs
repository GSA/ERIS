using AutoMapper;
using ERIS.Lookups;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace ERIS.Data
{
    internal class LoadLookupData
    {
        //Reference to logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IMapper lookupMapper;

        public LoadLookupData(IMapper mapper)
        {
            lookupMapper = mapper;

            lookupMapper.ConfigurationProvider.CompileMappings();
        }

        public Lookup GetEmployeeLookupData()
        {
            Lookup lookups = new Lookup();

            return lookups;
        }

        private Lookup MapEmployeeLookupData(MySqlDataReader lookupData)
        {
            Lookup lookup = new Lookup();

            //lookup_country
            lookupData.NextResult();
            lookup.countryLookup = lookupMapper.Map<IDataReader, List<CountryLookup>>(lookupData);

            //lookup_state
            lookupData.NextResult();
            lookup.stateLookup = lookupMapper.Map<IDataReader, List<StateLookup>>(lookupData);

            //lookup_region
            lookupData.NextResult();
            lookup.regionLookup = lookupMapper.Map<IDataReader, List<RegionLookup>>(lookupData);

            //lookup_building
            lookupData.NextResult();
            lookup.BuildingLookup = lookupMapper.Map<IDataReader, List<BuildingLookup>>(lookupData);

            return lookup;
        }
    }
}