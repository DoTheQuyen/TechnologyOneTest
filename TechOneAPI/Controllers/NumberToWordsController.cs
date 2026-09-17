using Microsoft.AspNetCore.Mvc;
using TechOneCore.Services.Interface;

namespace TechOneAPI.Controllers
{
    /// <summary>
    /// requirement: simple APIs, no authentication
    /// Every logic should be in the service layer, and the controller should only handle the request and response.
    /// </summary>
    public class NumberToWordsController : BaseApiController
    {
        private readonly INumberToWordsConverter _converter;
        private readonly ILogger<NumberToWordsController> _logger;

        public NumberToWordsController(INumberToWordsConverter converter, ILogger<NumberToWordsController> logger)
        {
            _converter = converter;
            _logger = logger;
        }

        /// <summary>
        /// requirement: convert an input numerical value to its corresponding English words representation
        /// so can ignore all other input type like string, char, etc.
        /// the best type for money is decimal, so we can use decimal as input type.
        /// did not mention about the range of the input value, so assume the input value can be any decimal value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("convert-to-words")]
        public IActionResult ConvertToWords([FromQuery] decimal? value)
        {
            return HandleRequest(_logger, "Error converting number to words",
                () => _converter.ConvertValueToString(value));
        }
    }
}