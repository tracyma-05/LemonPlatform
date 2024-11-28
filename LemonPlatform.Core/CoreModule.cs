using LemonPlatform.Core.Commons;
using LemonPlatform.Core.Databases;
using LemonPlatform.Core.Infrastructures.Dependency;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl.AdoJobStore;
using System.Reflection;

namespace LemonPlatform.Core
{
    public static class CoreModule
    {
        public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Assembly service register
            var coreAssembly = Assembly.GetAssembly(typeof(ILemonModule))!;
            services.AddServiceAssembly(coreAssembly);

            //Db Context
            var connectionString = new SqliteConnectionStringBuilder
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                ForeignKeys = true,
                DataSource = LemonConstants.DbName,
                Cache = SqliteCacheMode.Shared,
            }.ToString();

            services.AddDbContext<LemonDbContext>(options =>
            {
                options.UseSqlite(connectionString)
                    .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            });

            //Quartz
            services.AddQuartz(options =>
            {
                options.UsePersistentStore(s =>
                {
                    s.UseProperties = true;
                    s.UseMicrosoftSQLite(sqlite =>
                    {
                        sqlite.UseDriverDelegate<SQLiteDelegate>();
                        sqlite.ConnectionString = connectionString;
                    });

                    s.UseSystemTextJsonSerializer();
                });
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });
        }
    }
}