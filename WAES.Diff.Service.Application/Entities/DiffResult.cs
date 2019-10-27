using System.Collections.Generic;
using WAES.Diff.Service.Domain.Enums;

namespace WAES.Diff.Service.Domain.Entities
{
    public class DiffResult
    {
        public DiffStatus Status { get; set; }
        public IEnumerable<DiffDetail> Differences { get; set; }
    }
}
