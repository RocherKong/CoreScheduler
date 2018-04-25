using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreScheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreSchedule_ConsoleTest
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            JobManager.JobStart += JobManager_JobStart;
            JobManager.JobException += JobManager_JobException;
            JobManager.JobEnd += JobManager_JobEnd;
            JobManager.Initialize(new MyRegister());
        }

        private void JobManager_JobEnd(CoreScheduler.Event.JobEndedEvent obj)
        {
            Console.WriteLine("Job End:" + obj.Duration);
        }

        private void JobManager_JobException(CoreScheduler.Event.JobExceptionRaisedEvent obj)
        {
            Console.WriteLine("Job Error:" + obj.Message);
        }

        private void JobManager_JobStart(CoreScheduler.Event.JobStartedEvent obj)
        {
            Console.WriteLine("Job Start");
        }
    }
}
