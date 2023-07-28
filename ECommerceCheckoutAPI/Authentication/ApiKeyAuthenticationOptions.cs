using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ECommerceCheckoutAPI.Authentication
{
    /// <summary>
    /// Represents the options for the API key authentication scheme.
    /// </summary>
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// The authentication scheme name for API key authentication.
        /// </summary>
        public const string ApiKeyAuthenticationScheme = "ApiKey";

        /// <summary>
        /// Gets or sets the API key used for authentication.
        /// </summary>
        public string ApiKey { get; set; }
    }

    /// <summary>
    /// Custom authentication handler for API key authentication.
    /// </summary>
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeyAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">The monitor for the API key authentication options.</param>
        /// <param name="logger">The logger factory used for logging.</param>
        /// <param name="encoder">The URL encoder.</param>
        /// <param name="clock">The system clock.</param>
        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        /// <summary>
        /// Handles the authentication process for API key authentication.
        /// </summary>
        /// <returns>A task that represents the asynchronous authentication operation.</returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("ApiKey", out var apiKeyHeaderValues))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization header not found."));
            }

            var providedApiKey = apiKeyHeaderValues.ToString().Trim();
            var expectedApiKey = Options.ApiKey;

            if (string.IsNullOrEmpty(providedApiKey) || !providedApiKey.Equals(expectedApiKey, StringComparison.InvariantCultureIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));
            }

            var identity = new ClaimsIdentity(Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
