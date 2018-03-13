using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Cultivate.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace API_Cultivate
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
            services.AddMvc();
            services
                .AddScoped<IGameplayService>(serviceProvider => new GameplayService(serviceProvider.GetService<IPlayerRepository>(), serviceProvider.GetService<IRandomService>()))
                .AddScoped<IPlayerService>(serviceProvider => new PlayerService(serviceProvider.GetService<IPlayerRepository>()))
                .AddScoped<IUserService>(serviceProvider => new UserService(serviceProvider.GetService<IUserRepository>()))
                .AddScoped<IPlayerRepository>(serviceProvider => new PlayerRepository())
                .AddScoped<IUserRepository>(serviceProvider => new UserRepository())
                .AddScoped<IRandomService>(serviceProvider => new RandomService());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
