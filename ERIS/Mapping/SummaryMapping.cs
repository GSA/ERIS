using CsvHelper.Configuration;
using ERIS.Models;

namespace ERIS.Mapping
{
    internal sealed class ProcessedSummaryMapping : ClassMap<ProcessedSummary>
    {
        public ProcessedSummaryMapping()
        {
            Map(m => m.MonsterID).Name("Monster ID");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.MiddleName).Name("Middle Name");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.SocialSecurityNumber).Name("Social Security Number");
            Map(m => m.DateOfBirth).Name("Date of Birth");
            Map(m => m.Action).Name("Action");
        }
    }

    internal sealed class CreatedSummaryMapping : ClassMap<CreatedSummary>
    {
        public CreatedSummaryMapping()
        {
            Map(m => m.MonsterID).Name("Monster ID");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.MiddleName).Name("Middle Name");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.SocialSecurityNumber).Name("Social Security Number");
            Map(m => m.DateOfBirth).Name("Date of Birth");
        }
    }

    internal sealed class UpdatedSummaryMapping : ClassMap<UpdatedSummary>
    {
        public UpdatedSummaryMapping()
        {
            Map(m => m.MonsterID).Name("Monster ID");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.MiddleName).Name("Middle Name");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.SocialSecurityNumber).Name("Social Security Number");
            Map(m => m.DateOfBirth).Name("Date of Birth");
        }
    }

    internal sealed class ReviewedSummaryMapping : ClassMap<ReviewedSummary>
    {
        public ReviewedSummaryMapping()
        {
            Map(m => m.MonsterID).Name("Monster ID");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.MiddleName).Name("Middle Name");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.SocialSecurityNumber).Name("Social Security Number");
            Map(m => m.DateOfBirth).Name("Date of Birth");
        }
    }

    internal sealed class FlaggedSummaryMapping : ClassMap<FlaggedSummary>
    {
        public FlaggedSummaryMapping()
        {
            Map(m => m.MonsterID).Name("Monster ID");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.MiddleName).Name("Middle Name");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.SocialSecurityNumber).Name("Social Security Number");
            Map(m => m.DateOfBirth).Name("Date of Birth");
        }
    }
}
