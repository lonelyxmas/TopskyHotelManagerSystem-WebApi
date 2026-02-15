using EOM.TSHotelManagement.Common;
using EOM.TSHotelManagement.Domain;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System.Linq.Expressions;

namespace EOM.TSHotelManagement.Data
{
    public class GenericRepository<T> : SimpleClient<T> where T : class, new()
    {
        /// <summary>
        /// HTTP上下文访问器
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly JWTHelper _jWTHelper;

        public GenericRepository(ISqlSugarClient client, IHttpContextAccessor httpContextAccessor, JWTHelper jWTHelper) : base(client)
        {
            base.Context = client;
            _httpContextAccessor = httpContextAccessor;
            _jWTHelper = jWTHelper;
        }

        private string GetCurrentUser()
        {
            try
            {
                var authHeader = _httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"];
                if (string.IsNullOrEmpty(authHeader)) return "System";

                var token = authHeader.ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);

                return _jWTHelper.GetSerialNumber(token);
            }
            catch
            {
                return "System";
            }
        }

        public override bool Insert(T entity)
        {
            if (entity is BaseEntity baseEntity)
            {
                var currentUser = GetCurrentUser();
                if (!baseEntity.DataInsDate.HasValue)
                    baseEntity.DataInsDate = DateTime.Now;
                if (string.IsNullOrEmpty(baseEntity.DataInsUsr))
                    baseEntity.DataInsUsr = currentUser;
            }
            return base.Insert(entity);
        }

        public override bool Update(T entity)
        {
            if (entity is BaseEntity baseEntity)
            {
                var currentUser = GetCurrentUser();
                if (!baseEntity.DataChgDate.HasValue)
                    baseEntity.DataChgDate = DateTime.Now;
                if (string.IsNullOrEmpty(baseEntity.DataChgUsr))
                    baseEntity.DataChgUsr = currentUser;
            }

            var primaryKeys = base.Context.EntityMaintenance.GetEntityInfo<T>().Columns
                .Where(it => it.IsPrimarykey)
                .Select(it => it.PropertyName)
                .ToList();

            if (primaryKeys.Count <= 1)
            {
                return base.Context.Updateable(entity)
                    .IgnoreColumns(true, false)
                    .ExecuteCommand() > 0;
            }

            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                var idValue = Convert.ToInt64(idProperty.GetValue(entity));

                if (idValue == 0)
                {
                    var otherPrimaryKeys = primaryKeys.Where(pk => pk != "Id").ToList();

                    var parameter = Expression.Parameter(typeof(T), "it");
                    Expression whereExpression = null;

                    foreach (var key in otherPrimaryKeys)
                    {
                        var property = Expression.Property(parameter, key);
                        var value = entity.GetType().GetProperty(key).GetValue(entity);
                        var constant = Expression.Constant(value);
                        var equal = Expression.Equal(property, constant);

                        whereExpression = whereExpression == null
                            ? equal
                            : Expression.AndAlso(whereExpression, equal);
                    }

                    if (whereExpression != null)
                    {
                        var lambda = Expression.Lambda<Func<T, bool>>(whereExpression, parameter);

                        return base.Context.Updateable(entity)
                            .Where(lambda)
                            .IgnoreColumns(true, false)
                            .ExecuteCommand() > 0;
                    }
                }
            }

            return base.Context.Updateable(entity)
                    .IgnoreColumns(true, false)
                    .ExecuteCommand() > 0;
        }

        public override bool UpdateRange(List<T> updateObjs)
        {
            foreach (var entity in updateObjs)
            {
                if (entity is BaseEntity baseEntity)
                {
                    var currentUser = GetCurrentUser();
                    if (!baseEntity.DataChgDate.HasValue)
                        baseEntity.DataChgDate = DateTime.Now;
                    if (string.IsNullOrEmpty(baseEntity.DataChgUsr))
                        baseEntity.DataChgUsr = currentUser;
                }
            }

            return base.Context.Updateable(updateObjs)
                .IgnoreColumns(ignoreAllNullColumns: true)
                .ExecuteCommand() > 0;
        }

        public bool SoftDelete(T entity)
        {
            if (entity is BaseEntity baseEntity)
            {
                var currentUser = GetCurrentUser();
                if (!baseEntity.DataChgDate.HasValue)
                    baseEntity.DataChgDate = DateTime.Now;
                if (string.IsNullOrEmpty(baseEntity.DataChgUsr))
                    baseEntity.DataChgUsr = currentUser;
            }

            var primaryKeys = base.Context.EntityMaintenance.GetEntityInfo<T>().Columns
                .Where(it => it.IsPrimarykey)
                .Select(it => it.PropertyName)
                .ToList();

            if (primaryKeys.Count <= 1)
            {
                return base.Context.Updateable(entity)
                    .IgnoreColumns(ignoreAllNullColumns: true, false, true)
                    .ExecuteCommand() > 0;
            }

            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                var idValue = Convert.ToInt64(idProperty.GetValue(entity));

                if (idValue == 0)
                {
                    var otherPrimaryKeys = primaryKeys.Where(pk => pk != "Id").ToList();

                    var parameter = Expression.Parameter(typeof(T), "it");
                    Expression whereExpression = null;

                    foreach (var key in otherPrimaryKeys)
                    {
                        var property = Expression.Property(parameter, key);
                        var value = entity.GetType().GetProperty(key).GetValue(entity);
                        var constant = Expression.Constant(value);
                        var equal = Expression.Equal(property, constant);

                        whereExpression = whereExpression == null
                            ? equal
                            : Expression.AndAlso(whereExpression, equal);
                    }

                    if (whereExpression != null)
                    {
                        var lambda = Expression.Lambda<Func<T, bool>>(whereExpression, parameter);

                        return base.Context.Updateable(entity)
                            .Where(lambda)
                            .IgnoreColumns(ignoreAllNullColumns: true)
                            .ExecuteCommand() > 0;
                    }
                }
            }

            return base.Context.Updateable(entity)
                .IgnoreColumns(ignoreAllNullColumns: true)
                .ExecuteCommand() > 0;
        }

        public bool SoftDeleteRange(List<T> entities)
        {
            if (entities == null || !entities.Any())
                return false;

            var currentUser = GetCurrentUser();
            var now = DateTime.Now;
            var hasBaseEntity = false;

            // 更新内存中的实体状态
            foreach (var entity in entities)
            {
                if (entity is BaseEntity baseEntity)
                {
                    hasBaseEntity = true;
                    baseEntity.IsDelete = 1;
                    baseEntity.DataChgDate = now;
                    baseEntity.DataChgUsr = currentUser;
                }
            }

            if (!hasBaseEntity)
                return false;

            // 分批次处理
            const int batchSize = 1000;
            var totalAffected = 0;

            for (int i = 0; i < entities.Count; i += batchSize)
            {
                var batch = entities.Skip(i).Take(batchSize).ToList();

                totalAffected += base.Context.Updateable(batch)
                    .UpdateColumns(new[] {
                nameof(BaseEntity.IsDelete),
                nameof(BaseEntity.DataChgUsr),
                nameof(BaseEntity.DataChgDate)
                    })
                    .ExecuteCommand();
            }

            return totalAffected > 0;
        }
    }
}
