using System.ComponentModel.DataAnnotations;

namespace WAES.Diff.Service.Web.Models.Requests
{
    public class DiffRequest
    {
        /// <summary>
        /// Base64 enconded data
        /// </summary>
        [Required]
        public string Data { get; set; }
    }
}
