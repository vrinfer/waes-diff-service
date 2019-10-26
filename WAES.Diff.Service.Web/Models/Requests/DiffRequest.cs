using System.ComponentModel.DataAnnotations;

namespace WAES.Diff.Service.Web.Models.Requests
{
    public class DiffRequest
    {
        [Required]
        public string Data { get; set; }
    }
}
