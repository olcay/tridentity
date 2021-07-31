using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Mime;

namespace OtomatikMuhendis.TRIdentity.Api.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder ConfigureModelStateResponse(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var responseMessage = string.Join(" ", context.ModelState.Select(entry =>
                        string.Join("", entry.Value.Errors.Select(error => error.ErrorMessage))));

                    var result = new BadRequestObjectResult(new Response { Message = responseMessage });
                    result.ContentTypes.Add(MediaTypeNames.Application.Json);

                    return result;
                };
            });

            return mvcBuilder;
        }
    }
}
