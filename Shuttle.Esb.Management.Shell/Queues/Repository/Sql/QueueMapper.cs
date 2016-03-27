using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Esb.Management.Shell
{
    public class QueueMapper : IDataRowMapper<Queue>
    {
        public MappedRow<Queue> Map(DataRow row)
        {
            return new MappedRow<Queue>(row, new Queue
                                                 {
                                                     Uri = QueueColumns.Uri.MapFrom(row)
                                                 });
        }
    }
}
