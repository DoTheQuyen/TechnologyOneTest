using Microsoft.AspNetCore.Mvc;
using TechOneAPI.DTOs;

namespace TechOneAPI.Controllers
{
    /// <summary>
    /// implement a base API controller to handle common functionality for all API controllers
    /// implement a method to handle requests and return appropriate responses based on the result of the action
    /// return a standardized response structure using the ResponseDTO class, which includes success status, result data, and error messages
    /// benefit: this approach promotes code reusability, reduces duplication, and ensures consistent error handling across all API endpoints,
    /// and frontend can handle the response easily, as it can always expect a standardized response structure regardless of the data type being returned.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult RequestResponse<T>(ILogger logger, string errorContext, Func<T> action)
        {
            try
            {
                return Ok(ResponseDTO<T>.Success(action()));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseDTO<T>.Failure(ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, errorContext);
                return StatusCode(500, ResponseDTO<T>.Failure("An unexpected error occurred"));
            }
        }
    }
}