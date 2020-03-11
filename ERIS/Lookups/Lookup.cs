using System.Collections.Generic;

namespace ERIS.Lookups
{
    public class Lookup
    {
        public List<RegionLookup> regionLookup { get; set; }        

        public List<CountryLookup> countryLookup { get; set; }

        public List<StateLookup> stateLookup { get; set; }

        public List<BuildingLookup> BuildingLookup { get; set; }
    }
}
