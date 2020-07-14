using System.Collections.Generic;

namespace ERIS.Lookups
{
    public class Lookup
    {
        public List<RegionLookup> regionLookup { get; set; }        

        public List<CountryLookup> countryLookup { get; set; }

        public List<StateLookup> usStateLookup { get; set; }

        public List<StateLookup> mxStateLookup { get; set; }

        public List<StateLookup> caStateLookup { get; set; }

        public List<BuildingLookup> BuildingLookup { get; set; }
    }
}
