using Acubec.Solutions.Core;
using Acubec.Solutions.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opus.Victus.MMS.Core
{
    internal sealed class CoreContext : Context
    {
        public CoreContext(IServiceCollection containerCollection, IConfiguration configroot)
            : base(containerCollection, configroot)
        {

        }
    }
    internal static class CoreContextProvider
    {
        public static IServiceCollection AddCoreGateway(this IServiceCollection services, IConfiguration configuration)
        {
            ContextFactory.RegisterObject(new CoreContext(services, configuration));


            var mapper = new AcubecMapper(new CoreMapper());
            services.AddSingleton(typeof(IMapper), mapper);
            services.AddUtilities();

            return services;
        }

        public static IApplicationBuilder UseCoreGateway(this IApplicationBuilder app)
        {
            return app;
        }

        
    }

    internal sealed class CoreMapper : MapperConfiguration
    {
        public override void CreateProfile()
        {

        }
    }
}
