using System.Collections.Generic;

namespace MERS.Lookups
{
    public class Lookup
    {
        public List<RegionLookup> regionLookup { get; set; }        

        public List<CountryLookup> countryLookup { get; set; }

        public List<StateLookup> stateLookup { get; set; }

        public List<BuildingLookup> BuildingLookup { get; set; }
    }
}
