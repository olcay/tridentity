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

        [HttpGet("validate")]
        public Response Validate([FromRoute, Required, StringLength(11, MinimumLength = 11)] string identityNumber)
        {
            var identity = new Identity(identityNumber);

            return new Response { IsValid = identity.IsNumberValid() };
        }

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
