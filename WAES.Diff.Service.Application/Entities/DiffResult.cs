using System.Collections.Generic;
using WAES.Diff.Service.Common.Enums;

namespace WAES.Diff.Service.Domain.Entities
{
    public class DiffResult
    {
        public DiffStatus Status { get; set; }
        public IEnumerable<Diff> Differences { get; set; }
    }
}
