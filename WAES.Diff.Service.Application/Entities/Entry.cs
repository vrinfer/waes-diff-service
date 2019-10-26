using System;

namespace WAES.Diff.Service.Domain.Entities
{
    public class Entry
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public string LeftSide { get; set; }
        public string RightSide { get; set; }
    }
}
