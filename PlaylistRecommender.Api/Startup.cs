using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlaylistRecommender.Api.JsonContract;
using PlaylistRecommender.Domain.Handlers;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using PlaylistRecommender.Domain.Commands;
using Newtonsoft.Json;
using System.Text;

namespace PlaylistRecommender.Api
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
            services.AddControllers().AddNewtonsoftJson(options => {
                options.SerializerSettings.ContractResolver = new FluntJsonResolver();
            });

            services.AddTransient<PlaylistRecommendationHandler, PlaylistRecommendationHandler>();
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Playlist Recommender", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) => 
            {
                await next();
                if(context.Response.StatusCode == 404)
                {
                    var jsonResult = JsonConvert.SerializeObject(new CommandResult(false, "A rota solicitada nao foi encontrada"),
                                                                 Newtonsoft.Json.Formatting.None,
                                                                 new JsonSerializerSettings { 
                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                });
            
                    var response = Encoding.UTF8.GetBytes(jsonResult);
                    await context.Response.Body.WriteAsync(response, 0, response.Length);
                }
            });
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Playlist Recommender V1"); 
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
