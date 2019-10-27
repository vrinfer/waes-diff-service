using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Common;
using WAES.Diff.Service.Common.Exceptions;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Enums;
using WAES.Diff.Service.Domain.Interfaces.Services;
using WAES.Diff.Service.Web.Models.Requests;

namespace WAES.Diff.Service.Web.Controllers
{
    [Produces("application/json")]
    [Route("v1/diff")]
    [ApiController]
    public class DiffController : ControllerBase
    {
        private readonly IEntryService _entryService;
        private readonly IDiffService _diffService;

        public DiffController(IEntryService entryService, IDiffService diffService)
        {
            _entryService = entryService;
            _diffService = diffService;
        }

        /// <summary>
        /// Sets the left side to diff out
        /// </summary>
        /// <returns>If the item was added</returns>
        /// <param name="id">The id to identify the two sides from the diff</param>
        /// <param name="request">The binary data to compare encoded in base64</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("{id}/left")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SetDiffLeft(Guid id, [FromBody] DiffRequest request)
        {
            return await SetDiffSide(id, request, Side.Left);
        }

        /// <summary>
        /// Sets the left side to diff out
        /// </summary>
        /// <param name="id">The id to identify the two sides from the diff</param>
        /// <param name="request">The binary data to compare encoded in base64</param>
        /// <returns>If the item was added</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("{id}/right")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SetDiffRight(Guid id, [FromBody] DiffRequest request)
        {
            return await SetDiffSide(id, request, Side.Right);
        }

        /// <summary>
        /// Gets the diff between the left and right side for the data corresponding to the given id
        /// </summary>
        /// <param name="id">The id to identify the two sides from the diff</param>
        /// <returns>Whether the data in both sides is equal, differs in size or the offset and length of the differences</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DiffResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDiff(Guid id)
        {
            try
            {
                var result = await _diffService.GetDiff(id);

                return Ok(result);
            }
            catch(EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (InvalidInputException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.UNEXPECTED_ERROR);
            }
        }

        private async Task<ActionResult> SetDiffSide(Guid id, DiffRequest request, Side side)
        {
            try
            {
                await _entryService.AddSideToCompare(id, request.Data, side);

                return Ok();
            }
            catch (InvalidInputException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.UNEXPECTED_ERROR);
            }
        }
    }
}