using Acubec.Solutions.Core;
using Acubec.Solutions.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opus.Victus.Gateway
{
    internal sealed class GatewayContext : Context
    {
        public GatewayContext(IServiceCollection containerCollection, IConfiguration configroot) 
            : base(containerCollection, configroot)
        {
        }
    }

    internal static class ContextProvider
    {
        public static IServiceCollection AddGateway(this IServiceCollection services, IConfiguration configuration)
        {
            ContextFactory.RegisterObject(new GatewayContext(services, configuration));


            var mapper = new AcubecMapper(new GatewayMapper());
            services.AddSingleton(typeof(IMapper), mapper);
            services.AddUtilities();

            services.AddHttpClient("SecurityFactory", client =>
            {
                client.BaseAddress = new Uri(configuration["SecurityBaseURL"]);
            });

            return services;
        }

        public static IApplicationBuilder UseGateway(this IApplicationBuilder app)
        {
            ContextFactory.Current.Logger.WriteDebugEntry("AppStart", () => "Starting the Gateway Application");
            return app;
        }
    }

    internal sealed class GatewayMapper : MapperConfiguration
    {
        public override void CreateProfile()
        {

        }
    }
}
