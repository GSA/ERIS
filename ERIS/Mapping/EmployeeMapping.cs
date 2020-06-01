using CsvHelper.Configuration;
using ERIS.Models;
using System.Collections.Generic;

namespace ERIS.Mapping
{
    public sealed class EmployeeMapping : ClassMap<Employee>
    {
        public EmployeeMapping()
        {
            References<PersonMap>(r => r.Person);
            References<AddressMap>(r => r.Address);
            References<BuildingMap>(r => r.Building);
            References<BirthMap>(r => r.Birth);
            References<PositionMap>(r => r.Position);
            References<PhoneMap>(r => r.Phone);            
        }
    }

    public sealed class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Map(m => m.MonsterID).Index(MonsterConstants.MONSTER_NUMBER);
            Map(m => m.FirstName).Index(MonsterConstants.FIRST_NAME);
            Map(m => m.MiddleName).Index(MonsterConstants.MIDDLE_NAME);
            Map(m => m.LastName).Index(MonsterConstants.LAST_NAME);
            Map(m => m.Suffix).Index(MonsterConstants.SUFFIX);
            Map(m => m.SocialSecurityNumber).Index(MonsterConstants.SOCIAL_SECURITY_NUMBER);
            Map(m => m.Gender).Index(MonsterConstants.GENDER);
            Map(m => m.HomeEmail).Index(MonsterConstants.PERSONAL_EMAIL);
        }
    }

    public sealed class AddressMap : ClassMap<Address>
    {
        public AddressMap()
        {
            Map(m => m.HomeAddress1).Index(MonsterConstants.HOME_ADDRESS_1);
            Map(m => m.HomeAddress2).Index(MonsterConstants.HOME_ADDRESS_2);
            Map(m => m.HomeAddress3).Index(MonsterConstants.HOME_ADDRESS_3);
            Map(m => m.HomeCity).Index(MonsterConstants.HOME_CITY);
            Map(m => m.HomeState).Index(MonsterConstants.HOME_STATE);
            Map(m => m.HomeCountry).Index(MonsterConstants.HOME_COUNTRY);
            Map(m => m.HomeZipCode).Index(MonsterConstants.HOME_ZIP_CODE);
        }
    }

    public sealed class BuildingMap : ClassMap<Building>
    {
        public BuildingMap()
        {
            Map(m => m.BuildingLocationCode).Index(MonsterConstants.BUILDING_LOCATION_CODE);           
        }
    }

    public sealed class BirthMap : ClassMap<Birth>
    {
        public BirthMap()
        {
            Map(m => m.CityOfBirth).Index(MonsterConstants.BIRTH_CITY);
            Map(m => m.StateOfBirth).Index(MonsterConstants.BIRTH_STATE);
            Map(m => m.CountryOfBirth).Index(MonsterConstants.BIRTH_COUNTRY);
            Map(m => m.CountryOfCitizenship).Index(MonsterConstants.COUNTRY_OF_CITIZENSHIP);
            Map(m => m.Citizen).Index(MonsterConstants.CITIZEN);
            Map(m => m.DateOfBirth).Index(MonsterConstants.DATE_OF_BIRTH).TypeConverter<DateConverter>();
        }
    }

    public sealed class PositionMap : ClassMap<Position>
    {
        public PositionMap()
        {             
            Map(m => m.JobTitle).Index(MonsterConstants.JOB_TITLE);
            Map(m => m.Region).Index(MonsterConstants.REGION).TypeConverter<RegionConverter>();
            Map(m => m.IsVirtual).Index(MonsterConstants.VIRTUAL);
            Map(m => m.VirtualRegion).Index(MonsterConstants.VIRTUAL_REGION);
            Map(m => m.MajorOrg).Index(MonsterConstants.MAJOR_ORG).TypeConverter<MajorOrgConverter>();
           
        }
    }

    public sealed class PhoneMap : ClassMap<Phone>
    {
        public PhoneMap()
        {          
            Map(m => m.HomePhone).Index(MonsterConstants.HOME_PHONE);
            Map(m => m.PersonalCell).Index(MonsterConstants.PERSONAL_CELL);          
        }
    }
}
