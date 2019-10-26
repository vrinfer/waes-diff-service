using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Web.Models.Requests;
using WAES.Diff.Service.Web.Models.Responses;

namespace WAES.Diff.Service.Web.Controllers
{
    [Produces("application/json")]
    [Route("v1/diff")]
    [ApiController]
    public class DiffController : ControllerBase
    {
        /// <summary>
        /// Sets the left side to diff out
        /// </summary>
        /// <returns>If the item was added</returns>
        /// <param name="id">The id to identify the two sides from the diff</param>
        /// <param name="request">The binary data to compare encoded in base64</param>
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("{id}/left")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<ActionResult> SetDiffLeft(DiffRequest request, Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the left side to diff out
        /// </summary>
        /// <param name="id">The id to identify the two sides from the diff</param>
        /// <param name="request">The binary data to compare encoded in base64</param>
        /// <returns>If the item was added</returns>
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("{id}/right")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<ActionResult> SetDiffRight(Guid id, [FromBody] DiffRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the diff between the left and right side for the data corresponding to the given id
        /// </summary>
        /// <param name="id">The id to identify the two sides from the diff</param>
        /// <returns>Whether the data in both sides is equal, differs in size or the offset and length of the differences</returns>
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DiffResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> GetDiff(Guid id, [FromBody] DiffRequest request)
        {
            throw new NotImplementedException();
        }
    }
}