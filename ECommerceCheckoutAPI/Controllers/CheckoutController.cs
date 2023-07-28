using System;
using System.Collections.Generic;
using ECommerceCheckout.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerceCheckoutAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(ICheckoutService checkoutService, ILogger<CheckoutController> logger)
        {
            _checkoutService = checkoutService ?? throw new ArgumentNullException(nameof(checkoutService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(400)]
        public ActionResult<decimal> Post([FromBody] List<string> watchIds)
        {
            if (watchIds == null || watchIds.Count == 0)
            {
                return BadRequest("No watch IDs provided.");
            }

            try
            {
                decimal totalCost = _checkoutService.CalculateTotalCost(watchIds);
                return Ok(new { total_cost = totalCost });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during the checkout process.");
                return StatusCode(500, "An unexpected error occurred during the checkout process.");
            }
        }
    }
}
