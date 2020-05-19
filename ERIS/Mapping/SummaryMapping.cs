using CsvHelper.Configuration;
using ERIS.Models;

namespace ERIS.Mapping
{
    internal sealed class CreatedSummaryMapping : ClassMap<CreatedSummary>
    {
        public CreatedSummaryMapping()
        {
            Map(m => m.employeeData).Name("Create New Records");
        }
    }

    internal sealed class UpdatedSummaryMapping : ClassMap<UpdatedSummary>
    {
        public UpdatedSummaryMapping()
        {
            Map(m => m.employeeData).Name("Update Records");
        }
    }

    internal sealed class ReviewedSummaryMapping : ClassMap<ReviewedSummary>
    {
        public ReviewedSummaryMapping()
        {
            Map(m => m.employeeData).Name("Review Records");
        }
    }

    internal sealed class FlaggedSummaryMapping : ClassMap<FlaggedSummary>
    {
        public FlaggedSummaryMapping()
        {
            Map(m => m.employeeData).Name("Flag Records");
        }
    }
}
