using EOM.TSHotelManagement.Common.Core;
using EOM.TSHotelManagement.Migration;
using EOM.TSHotelManagement.Shared;
using EOM.TSHotelManagement.WebApi.Constant;
using jvncorelib.EncryptorLib;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Oracle.ManagedDataAccess.Client;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EOM.TSHotelManagement.WebApi
{
    public class InitializeConfig
    {
        private readonly IConfiguration config;

        public InitializeConfig(IConfiguration configuration)
        {
            config = configuration;
        }

        #region initlize database
        public void InitializeDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var factory = scope.ServiceProvider.GetRequiredService<ISqlSugarClientFactory>();

            try
            {
                var dbName = config[DatabaseConstants.DefaultDatabase] ?? DatabaseConstants.MariaDB;
                var dbSettings = GetDatabaseSettings(config, dbName);

                if (dbSettings.DbType != DbType.Sqlite)
                {
                    using (var masterDb = CreateMasterConnection(config, dbName, dbSettings.DbType))
                    {
                        if (!masterDb.DbMaintenance.GetDataBaseList().Contains(dbSettings.Database))
                        {
                            Console.WriteLine($"Creating database {dbSettings.Database}...");
                            masterDb.DbMaintenance.CreateDatabase(dbSettings.Database,null);
                            Console.WriteLine("Database created successfully");
                        }
                    }
                }

                using (var db = factory.CreateClient(dbName))
                {
                    Console.WriteLine("Initializing database schema...");

                    var entityBuilder = new EntityBuilder();

                    var dbTables = db.DbMaintenance.GetTableInfoList()
                                    .Select(a => a.Name.Trim().ToLower())
                                    .ToList();

                    var initializeTables = entityBuilder.EntityTypes
                                    .Select(a => a.Name.Trim().ToLower())
                                    .ToList();

                    var needCreateTable = initializeTables
                                    .Except(dbTables, StringComparer.OrdinalIgnoreCase)
                                    .ToList();

                    var needCreateTableTypes = needCreateTable
                       .Select(tableName => entityBuilder.EntityTypes.SingleOrDefault(type => type.Name.Trim().Equals(tableName, StringComparison.OrdinalIgnoreCase)))
                       .Where(type => type != null)
                       .ToArray();

                    db.CodeFirst.InitTables(needCreateTableTypes);

                    Console.WriteLine("Database schema initialized");

                    SeedInitialData(db);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization failed:{ex.Message}");
                throw;
            }
        }

        private (string Database, DbType DbType) GetDatabaseSettings(IConfiguration config, string dbName)
        {
            var dbType = SqlSugarClientFactory.GetDbType(dbName);
            var connectionString = GetConnectionString(config, dbName);

            switch (dbType)
            {
                case DbType.MySql:
                case DbType.MySqlConnector:
                    var builder = new MySqlConnectionStringBuilder(connectionString);
                    return (builder.Database, dbType);
                case DbType.SqlServer:
                    var sqlBuilder = new SqlConnectionStringBuilder(connectionString);
                    return (sqlBuilder.InitialCatalog, dbType);
                case DbType.Oracle:
                    var oracleBuilder = new OracleConnectionStringBuilder(connectionString);
                    return (oracleBuilder.DataSource, dbType);
                case DbType.Sqlite:
                    var sqliteBuilder = new SqliteConnectionStringBuilder(connectionString);
                    return (sqliteBuilder.DataSource, dbType);
                default:
                    throw new NotSupportedException($"Unsupported DbType: {dbType}");
            }
        }

        private SqlSugarClient CreateMasterConnection(IConfiguration config, string dbName, DbType dbType)
        {
            var connectionString = GetConnectionString(config, dbName);

            dynamic builder = null;

            switch (dbType)
            {
                case DbType.MySql:
                case DbType.MySqlConnector:
                    builder = new MySqlConnectionStringBuilder(connectionString)
                    {
                        Database = null,
                    };
                    break;
                case DbType.SqlServer:
                    builder = new SqlConnectionStringBuilder(connectionString)
                    {
                        InitialCatalog = "master",
                        ConnectTimeout = 30
                    };
                    break;
                case DbType.Oracle:
                    builder = new OracleConnectionStringBuilder(connectionString)
                    {
                        UserID = "sys as sysdba",
                    };
                    break;
                case DbType.Sqlite:
                    break;
            }

            return new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = builder?.ConnectionString ?? string.Empty,
                DbType = dbType,
                IsAutoCloseConnection = true,
                ConfigureExternalServices = new ConfigureExternalServices
                {
                    EntityService = (c, p) =>
                    {
                        if (dbType == DbType.Oracle && p.IsPrimarykey && p.IsIdentity)
                            p.OracleSequenceName = "SEQ_" + c.Name;
                    }
                }
            });
        }

        private string GetConnectionString(IConfiguration config, string dbName)
        {
            if (Environment.GetEnvironmentVariable(DatabaseConstants.DockerEnv) != null)
            {
                return Environment.GetEnvironmentVariable($"{dbName}ConnectStr")
                    ?? throw new ArgumentException($"Environment variable {dbName}ConnectStr not found");
            }
            return config.GetConnectionString($"{dbName}ConnectStr");
        }

        private void SeedInitialData(ISqlSugarClient db)
        {
            Console.WriteLine("Initializing database data...");

            try
            {
                var entityBuilder = new EntityBuilder();
                var entitiesToAdd = new List<object>();

                foreach (var entityData in entityBuilder.GetEntityDatas())
                {
                    switch (entityData)
                    {
                        case Menu menu when db.Queryable<Menu>().Any(a => a.Key == menu.Key):
                            continue;

                        case Administrator admin when db.Queryable<Administrator>().Any(a => admin.Account == admin.Account):
                            continue;

                        case AdministratorType adminType when db.Queryable<AdministratorType>().Any(a => adminType.TypeName == adminType.TypeName):
                            continue;

                        case NavBar navBar when db.Queryable<NavBar>().Any(a => navBar.NavigationBarName == navBar.NavigationBarName):
                            continue;

                        case PromotionContent promoContent when db.Queryable<PromotionContent>().Any(a => promoContent.PromotionContentMessage == promoContent.PromotionContentMessage):
                            continue;

                        case SystemInformation sysInfo when db.Queryable<SystemInformation>().Any(a => sysInfo.UrlNumber == sysInfo.UrlNumber):
                            continue;
                    }
                    entitiesToAdd.Add(entityData);
                }

                if (entitiesToAdd.Count > 0)
                {
                    foreach (var data in entitiesToAdd)
                    {
                        db.InsertableByObject(data).ExecuteCommand();
                    }
                    Console.WriteLine($"Adding {entitiesToAdd.Count} initial data records");
                }
                else
                {
                    Console.WriteLine("No initial data to add");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database data initialization failed: {ex.Message}");
                throw;
            }
            finally
            {
                Console.WriteLine("Database data initialization completed");
                Console.WriteLine($"administrator account：admin\nadministrator password：admin");
            }
        }
        #endregion
    }
}
