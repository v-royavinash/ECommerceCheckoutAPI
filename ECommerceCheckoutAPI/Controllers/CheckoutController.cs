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
            // Implement the logic to calculate the total cost for a list of watches
            return 360;
        }
    }
}
