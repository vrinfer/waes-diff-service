using System.Collections.Generic;
using System.Linq;
using WAES.Diff.Service.Domain.Enums;

namespace WAES.Diff.Service.Domain.Entities
{
    public class DiffResult
    {
        public DiffStatus Status { get; set; }
        public IEnumerable<DiffDetail> Differences { get; set; }

        public DiffResult() { }

        /// <summary>
        /// Creates a DiffResult instance setting the Status based on the amount of differences
        /// </summary>
        /// <param name="differences"></param>
        public DiffResult(IEnumerable<DiffDetail> differences)
        {
            Status = differences.Any() ? DiffStatus.NotEqual : DiffStatus.Equal;
            Differences = differences.Any() ? differences : null;
        }

        /// <summary>
        /// Creates a DiffResult instance without differences
        /// </summary>
        /// <param name="status"></param>
        public DiffResult(DiffStatus status)
        {
            Status = status;
            Differences = null;
        }
    }
}
