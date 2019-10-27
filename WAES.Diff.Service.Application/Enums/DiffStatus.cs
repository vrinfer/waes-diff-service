using System.ComponentModel;

namespace WAES.Diff.Service.Domain.Enums
{
    public enum DiffStatus
    {
        [Description("Equal")]
        Equal,

        [Description("Unmatched Size")]
        UnmatchedSize,

        [Description("Not Equal")]
        NotEqual
    }
}
