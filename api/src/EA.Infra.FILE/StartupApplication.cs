using EA.Infra.FILE.Context;
using EA.Infra.FILE.EventSourcing; 
using EA.Infra.FILE.Repository.EventSourcing;
using EA.NetDevPack.Events; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EA.NetDevPack;
using EA.NetDevPack.Mediator; 
using EA.Infra.FILE.Consul; 
using EA.NetDevPack.Context;
using EA.Infra.FILE.FileConfig;
using EA.Infra.FILE.Repository;
using EA.Domain.FILE.Interfaces;

namespace EA.Infra.FILE
{
    public  class StartupApplication: IStartupApplication
    {
        public int Priority => 2;
        public bool BeforeConfigure => true;

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddConsul(configuration);
            //services.AddDbContext<SqlCoreContext>(options =>options.UseSqlServer(configuration.GetConnectionString("CoreConnection")));
            var connectionStringPlaceHolder = configuration.GetConnectionString("FILEConnection");
            services.AddDbContext<SqlCoreContext>((serviceProvider, dbContextBuilder) =>
            {
                var context = serviceProvider.GetRequiredService<IContextUser>();
               //var connectionString = connectionStringPlaceHolder.Replace("{tenant}", "becoxy").Replace("{zone}", context.UserClaims.Data_Zone);
                //dbContextBuilder.UseSqlServer(connectionString);
                dbContextBuilder.UseSqlServer(connectionStringPlaceHolder);
            });

            //services.AddDbContext<EventStoreSqlContext>(options => options.UseSqlServer(configuration.GetConnectionString("EventConnection"))); 
            var connectionEventPlaceHolder = configuration.GetConnectionString("FILEEventConnection");
            services.AddDbContext<EventStoreSqlContext>((serviceProvider, dbContextBuilder) =>
            {
                var context = serviceProvider.GetRequiredService<IContextUser>();
                //var connectionString = connectionEventPlaceHolder.Replace("{tenant}", "becoxy").Replace("{zone}", context.UserClaims.Data_Zone);
                //dbContextBuilder.UseSqlServer(connectionString);
                dbContextBuilder.UseSqlServer(connectionEventPlaceHolder);
            });

            services.Configure<FileUploadConfig>(configuration.GetSection("FileUpload"));
            services.AddScoped<IMediatorHandler, EA.Infra.FILE.Bus.MediatorHandler>();
            services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<EventStoreSqlContext>();
            // Infra - Data

            services.AddScoped<SqlCoreContext>();
            services.AddRabbitMQ(configuration);
        }

        public void Configure(WebApplication application, IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                application.UseConsul(application.Lifetime);
            }
            catch (Exception)
            {
            }
        }
    }
}