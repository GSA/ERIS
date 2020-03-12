using CsvHelper.Configuration;
using MERS.Models;

namespace MERS.Mapping
{
    internal sealed class ProcessedSummaryMapping : ClassMap<ProcessedSummary>
    {
        public ProcessedSummaryMapping()
        {
            //This is an example, will need to be changed
            Map(m => m.ItemsProcessed).Name("Itesm Processed");            
        }
    }
}
