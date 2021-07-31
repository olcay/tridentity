using Microsoft.AspNetCore.Mvc;
using OtomatikMuhendis.TRIdentity.Api.Services;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace OtomatikMuhendis.TRIdentity.Api.Controllers
{
    [ApiController]
    [Route("identity/{identityNumber}")]
    [Produces(MediaTypeNames.Application.Json)]
    public class IdentityController : ControllerBase
    {
        private readonly NVIService _nviService;

        public IdentityController(NVIService nviService)
        {
            _nviService = nviService;
        }

        /// <summary>
        /// Validate an identity number
        /// </summary>
        /// <param name="identityNumber">An identity number</param>
        /// <returns>Validation result and a message in case of en exception</returns>
        [HttpGet("validate")]
        public Response Validate(
            [FromRoute, Required, StringLength(11, MinimumLength = 11)] string identityNumber)
        {
            var identity = new Identity(identityNumber);

            return new Response { IsValid = identity.IsNumberValid() };
        }

        /// <summary>
        /// Verify an identity number using the webservice
        /// </summary>
        /// <param name="identityNumber">An identity number</param>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        /// <param name="birthyear">Year</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Validation result and a message in case of en exception</returns>
        [HttpGet("verify")]
        public async Task<Response> Verify(
            [FromRoute, Required, StringLength(11, MinimumLength = 11)] string identityNumber, 
            [FromQuery, Required] string firstName, 
            [FromQuery, Required] string lastName, 
            [FromQuery, Required, Range(1000, 9999)] int birthyear,
            CancellationToken cancellationToken)
        {
            var identity = new Identity(identityNumber, firstName, lastName, birthyear);

            return new Response { IsValid = identity.IsNumberValid() && await _nviService.Verify(identity, cancellationToken) };
        }
    }
}
