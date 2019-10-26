namespace WAES.Diff.Service.Domain.Entities
{
    public class Entry
    {
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public string LeftSide { get; set; }
        public string RightSide { get; set; }
    }
}
