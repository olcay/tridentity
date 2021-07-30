using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OtomatikMuhendis.TRIdentity.Api.Middleware;
using OtomatikMuhendis.TRIdentity.Api.Services;
using System.Linq;
using System.Net.Mime;

namespace OtomatikMuhendis.TRIdentity.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "OtomatikMuhendis.TRIdentity.Api",
                        Version = "v1",
                        Contact = new OpenApiContact
                        {
                            Url = new System.Uri("https://olcay.dev"),
                            Name = "Olcay Bayram"
                        }
                    });
            });
            services.AddHttpClient<NVIService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OtomatikMuhendis.TRIdentity.Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
