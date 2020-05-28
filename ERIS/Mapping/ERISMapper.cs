using AutoMapper;
using AutoMapper.Data;
using ERIS.Lookups;
using ERIS.Models;

namespace ERIS.Mapping
{
    internal class ERISMapper
    {
        private MapperConfiguration lookupConfig;
        private MapperConfiguration dataConfig;

        public void CreateLookupConfig()
        {
            lookupConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Lookup, CountryLookup>().ReverseMap();
                cfg.CreateMap<Lookup, StateLookup>().ReverseMap();
                cfg.CreateMap<Lookup, RegionLookup>().ReverseMap();               
                cfg.CreateMap<Lookup, BuildingLookup>().ReverseMap();

                cfg.AddDataReaderMapping();
                cfg.AllowNullCollections = true;
            });
        }

        public void CreateDataConfig()
        {
            dataConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddDataReaderMapping();
                cfg.AllowNullCollections = true;

                cfg.CreateMap<Employee, Person>().ReverseMap();
                cfg.CreateMap<Employee, Address>().ReverseMap();
                cfg.CreateMap<Employee, Birth>().ReverseMap();                
                cfg.CreateMap<Employee, Position>().ReverseMap();
                cfg.CreateMap<Employee, Phone>().ReverseMap();
                cfg.CreateMap<Employee, Building>().ReverseMap();
            });
        }

        public IMapper CreateLookupMapping()
        {
            return lookupConfig.CreateMapper();
        }

        public IMapper CreateDataMapping()
        {
            return dataConfig.CreateMapper();
        }
    }
}
