using Acubec.Solutions.Core;
using Acubec.Solutions.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoFramework;
using Opus.Victus.SecurityInterface.Database;
using Opus.Victus.SecurityInterface.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opus.Victus.SecurityInterface
{
    internal sealed class SecurityContext : Context
    {
        public SecurityContext(IServiceCollection containerCollection, IConfiguration configroot)
            : base(containerCollection, configroot)
        {

        }
    }

    internal static class ContextProvider
    {
        public static IServiceCollection AddSecurityLayer(this IServiceCollection services, IConfiguration configuration)
        {
            ContextFactory.RegisterObject(new SecurityContext(services, configuration));
            var connection = MongoDbConnection.FromConnectionString(configuration["ConnectionString"]);

            services.AddTransient<IDataContext, SecurityDataContext>((s)
                => new SecurityDataContext(connection));
            
            var mapper = new AcubecMapper(new SecurityMapper());
            services.AddSingleton(typeof(IMapper), mapper);
            services.AddUtilities();
            return services;
        }

        public static IApplicationBuilder UseSecurityLayer(this IApplicationBuilder app)
        {
            ContextFactory.Current.Logger.WriteDebugEntry("AppStart", () => "Starting the Gateway Application");
            return app;
        }
    }

    internal sealed class SecurityMapper : MapperConfiguration
    {
        public override void CreateProfile()
        {
            CreateMap<UserDataEntity, UserEntity>((s, d) =>
            {
                if (d == null) d = new UserEntity(Guid.Parse(s.Id));
                d.Name = s.Name;
                d.Password = s.Password;
                d.PhoneNumber = s.PhoneNumber;
                d.EmailId = s.EmailId;
                return d;
            });

            CreateMap<UserEntity,UserDataEntity>((s, d) =>
            {
                if (d == null) d = new UserDataEntity();
                d.Id = s.Id.ToString();
                d.Name = s.Name;
                d.Password = s.Password;
                d.EmailId = s.EmailId;
                d.PhoneNumber = s.PhoneNumber;
                return d;
            });
        }
    }
}
