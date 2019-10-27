using System.Collections.Generic;
using System.Linq;
using WAES.Diff.Service.Domain.Enums;

namespace WAES.Diff.Service.Domain.Entities
{
    public class DiffResult
    {
        public DiffStatus Status { get; set; }
        public IEnumerable<DiffDetail> Differences { get; set; }


        public DiffResult(IEnumerable<DiffDetail> differences)
        {
            Status = differences.Any() ? DiffStatus.NotEqual : DiffStatus.Equal;
            Differences = differences.Any() ? differences : null;
        }

        public DiffResult(DiffStatus status)
        {
            Status = status;
        }
    }
}
