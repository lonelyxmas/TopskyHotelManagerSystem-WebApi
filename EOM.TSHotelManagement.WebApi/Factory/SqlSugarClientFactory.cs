using EOM.TSHotelManagement.Shared;
using EOM.TSHotelManagement.WebApi.Constant;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;

namespace EOM.TSHotelManagement.WebApi
{
    public class SqlSugarClientFactory : ISqlSugarClientFactory
    {
        private readonly IConfiguration _configuration;

        public SqlSugarClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ISqlSugarClient CreateClient(string dbName = null)
        {
            // 读取默认数据库名称
            dbName ??= _configuration[DatabaseConstants.DefaultDatabase];
            string connectionString;
            if (Environment.GetEnvironmentVariable(DatabaseConstants.DockerEnv) != null)
            {
                connectionString = Environment.GetEnvironmentVariable(DatabaseConstants.DefaultDatabase) switch
                {
                    DatabaseConstants.PgSql => Environment.GetEnvironmentVariable($"{DatabaseConstants.PgSql}ConnectStr"), //Test passed
                    DatabaseConstants.MySql => Environment.GetEnvironmentVariable($"{DatabaseConstants.MySql}ConnectStr"), //Test passed
                    DatabaseConstants.SqlServer => Environment.GetEnvironmentVariable($"{DatabaseConstants.SqlServer}ConnectStr"), //Test passed
                    DatabaseConstants.Oracle => Environment.GetEnvironmentVariable($"{DatabaseConstants.Oracle}ConnectStr"), //Please manually test
                    DatabaseConstants.MariaDB => Environment.GetEnvironmentVariable($"{DatabaseConstants.MariaDB}ConnectStr"), //Test passed
                    _ => throw new ArgumentException("Unsupported database", nameof(dbName)),
                };
            }
            else
            {
                connectionString = _configuration.GetConnectionString(dbName + "ConnectStr");
            }

            var dbType = GetDbType(dbName);

            ConnectionConfig config = new()
            {
                ConnectionString = connectionString,
                DbType = dbType,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                LanguageType = LanguageType.Chinese
            };

            switch (dbType)
            {
                case DbType.PostgreSQL:
                    config.MoreSettings = new ConnMoreSettings
                    {
                        PgSqlIsAutoToLower = false, //数据库存在大写字段的
                                                    //，需要把这个设为false ，并且实体和字段名称要一样
                                                    //如果数据库里的数据表本身就为小写，则改成true
                                                    //详细可以参考官网https://www.donet5.com/Home/Doc
                        DisableMillisecond = true
                    };
                    break;
                case DbType.SqlServer:
                    config.MoreSettings = new ConnMoreSettings()
                    {
                        SqlServerCodeFirstNvarchar = true,//建表字符串默认Nvarchar
                        IsWithNoLockQuery = true
                    };
                    break;
            }

            return new SqlSugarClient(config);
        }

        public static DbType GetDbType(string dbName)
        {
            return dbName switch
            {
                DatabaseConstants.PgSql => DbType.PostgreSQL,
                DatabaseConstants.MySql => DbType.MySql,
                DatabaseConstants.Oracle => DbType.Oracle,
                DatabaseConstants.SqlServer => DbType.SqlServer,
                DatabaseConstants.Sqlite => DbType.Sqlite,
                DatabaseConstants.MariaDB => DbType.MySqlConnector,
                _ => throw new ArgumentException("Unsupported database", nameof(dbName))
            };
        }
    }
}
