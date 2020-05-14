using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Acubec.Solutions.Utilities;
using Acubec.Solutions.Core;
using Acubec.Solutions.Core.Batch;

namespace Opus.Victus.MMS.Core
{

    public class EmptyTask : ITask
    {
        public EmptyTask() : base("EmptyTask")
        {
        }

        public override async Task<TaskCompletionParameter> Execute(TaskCompletionParameter taskCompletionParametr)
        {
            return await Task.FromResult<TaskCompletionParameter>(new TaskCompletionParameter()
            {
                LastRun = DateTime.Now.ToString(),
                LastRunOutPut = "",
                Name = "EmptyTask"
            });
        }
    }

    public class Schedule : ISchedule
    {
        public Schedule()
        {
            
        }

        public IJobSchedule[] GetJobSchedules()
        {
            var dictionary = new Dictionary<string, Type>();
            dictionary.Add("EmptyTask", typeof(EmptyTask));
            var jobScheduler = ContextFactory.Current.ResolveDependency<IJobScheduler>();
            jobScheduler.RegisterTaskFactory(dictionary);

            var schedule = new IJobSchedule[1];
            schedule[0] = ContextFactory.Current.ResolveDependency<IJobSchedule>();
            schedule[0].Name = "EmptyTask";
            schedule[0].EveryAfter = 30;
            schedule[0].Type = SchedulerEventType.EveryAfter;
            return schedule;
        }

        public void OnComplete(IJobSchedule js)
        {
            
        }
    }


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
            services.AddCoreGateway(Configuration);
            services.AddUtilities();
            services.AddJob(new Schedule());            
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            
            app.UseCors(options =>
            {
                options.AllowAnyHeader()
                       .AllowAnyOrigin()
                       .AllowAnyOrigin();
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseUtilities();
            app.UseCoreGateway();
            app.StartJob();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
