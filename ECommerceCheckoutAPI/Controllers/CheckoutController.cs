using System;
using System.Collections.Generic;
using ECommerceCheckout.Domain.Services;
using ECommerceCheckout.Utilities.Exceptions;
using ECommerceCheckoutAPI.Authentication;
using ECommerceCheckoutAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerceCheckoutAPI.Controllers
{
    /// <summary>
    /// Controller to handle checkout actions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.ApiKeyAuthenticationScheme)]

    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        private readonly ILogger<CheckoutController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutController"/> class.
        /// </summary>
        /// <param name="checkoutService">The checkout service.</param>
        /// <param name="logger">The logger.</param>
        public CheckoutController(ICheckoutService checkoutService, ILogger<CheckoutController> logger)
        {
            _checkoutService = checkoutService ?? throw new ArgumentNullException(nameof(checkoutService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Calculate the total cost for a list of watches.
        /// </summary>
        /// <param name="watchIds">The list of watch IDs to calculate the cost.</param>
        /// <returns>The total cost of the watches.</returns>
        /// <response code="200">Returns the total cost of the watches.</response>
        /// <response code="400">If the watchIds list is null or empty.</response>
        [HttpPost]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(400)]
        public ActionResult<WatchCatalogItem> Post([FromBody] List<string> watchIds)
        {
            if (watchIds == null || watchIds.Count == 0)
            {
                return BadRequest("No watch IDs provided.");
            }

            try
            {
                decimal totalCost = _checkoutService.CalculateTotalCost(watchIds);
                return Ok( new WatchCatalogItem() { TotalCost = totalCost });
            }
            catch (WatchNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Watch not found in the catalog: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during the checkout process.");
                return StatusCode(500, "An unexpected error occurred during the checkout process.");
            }
        }
    }
}
