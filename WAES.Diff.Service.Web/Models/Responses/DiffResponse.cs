using System.Collections.Generic;
using WAES.Diff.Service.Domain.Entities;

namespace WAES.Diff.Service.Web.Models.Responses
{
    public class DiffResponse
    {
        public DiffResults Result { get; set; }
        public IEnumerable<Diff> Differences { get; set; }
    }
}
