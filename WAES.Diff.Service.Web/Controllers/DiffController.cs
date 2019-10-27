using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Common.Exceptions;
using WAES.Diff.Service.Domain.Enums;
using WAES.Diff.Service.Domain.Interfaces;
using WAES.Diff.Service.Web.Models.Requests;
using WAES.Diff.Service.Web.Models.Responses;

namespace WAES.Diff.Service.Web.Controllers
{
    [Produces("application/json")]
    [Route("v1/diff")]
    [ApiController]
    public class DiffController : ControllerBase
    {
        private readonly IEntryService _entryService;
        private readonly IDiffService _diffService;
        private readonly IMapper _mapper;

        public DiffController(IEntryService entryService, IDiffService diffService, IMapper mapper)
        {
            _entryService = entryService;
            _diffService = diffService;
            _mapper = mapper;
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
            try
            {
                await _entryService.AddSideToCompare(id, request.Data, Side.Left);

                return Ok();
            }
            catch(InvalidInputException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected Error");
            }
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
            try
            {
                await _entryService.AddSideToCompare(id, request.Data, Side.Right);

                return Ok();
            }
            catch (InvalidInputException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected Error");
            }
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
        [ProducesResponseType(typeof(DiffResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDiff(Guid id)
        {
            try
            {
                var diff = await _diffService.GetDiff(id);

                var result = _mapper.Map<DiffResponse>(diff);

                return Ok(result);
            }
            catch(EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected Error");
            }
        }
    }
}