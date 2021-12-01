using System;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;


// bibliography
// https://medium.com/@giorgos.dyrrahitis/net-core-and-rabbitmq-5f3c76f39de6



namespace DataExplorerApi
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

            services.AddControllers();

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<IRequestHandler<Model.LogCommand, Unit>, Model.LogCommandHandler>();
            services.AddHostedService<QueueConsumer>();
            services.AddSingleton(serviceProvider =>
                {
                    var uri = new Uri("amqp://guest:guest@localhost:5672");
                    return new ConnectionFactory
                    {
                        Uri = uri,
                        DispatchConsumersAsync = true
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DataExplorerApi", Version = "v1" });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DataExplorerApi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
       

}
