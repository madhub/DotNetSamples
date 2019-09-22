using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CorsAspdemo
{
    /// <summary>
    //curl -I -H "Origin: http://example.com" -H "Access-Control-Request-Method: POST" -H "Access-Control-Request-Headers: X-Requested-With"  -X OPTIONS "http://localhost:10713/api/values"
    //HTTP/1.1 204 No Content
    //Date: Sun, 22 Sep 2019 11:46:08 GMT
    //Server: Kestrel
    //Vary: Origin
    //Access-Control-Allow-Headers: X-Requested-With
    //Access-Control-Allow-Methods: GET,POST
    //Access-Control-Allow-Origin: http://example.com
    /// </summary>
    /// 

    //curl -I -H "Origin: http://www.contoso.com" -H "Access-Control-Request-Method: POST" -H "Access-Control-Request-Headers: X-Requested-With"  -X OPTIONS "http://localhost:10713/api/values"
    //HTTP/1.1 204 No Content
    //Date: Sun, 22 Sep 2019 11:45:28 GMT
    //Server: Kestrel
    //Vary: Origin
    //Access-Control-Allow-Headers: X-Requested-With
    //Access-Control-Allow-Methods: GET,POST
    //Access-Control-Allow-Origin: http://www.contoso.com
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://example.com",
                                        "http://www.contoso.com")
                                        .AllowAnyHeader()
                                        .WithMethods("GET","POST");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(MyAllowSpecificOrigins);
            app.UseMvc();
        }
    }
}
