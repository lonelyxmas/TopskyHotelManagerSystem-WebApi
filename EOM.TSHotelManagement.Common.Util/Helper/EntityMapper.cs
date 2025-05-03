using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EOM.TSHotelManagement.Common.Util
{
    public static class EntityMapper
    {
        /// <summary>
        /// 映射单个实体
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>目标对象</returns>
        public static TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : new()
        {
            if (source == null) return default;

            var destination = new TDestination();

            var sourceProperties = typeof(TSource).GetProperties(
                BindingFlags.Public | BindingFlags.Instance);
            var destinationProperties = typeof(TDestination).GetProperties(
                BindingFlags.Public | BindingFlags.Instance);

            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties
                    .SingleOrDefault(p => p.Name.Equals(
                        sourceProperty.Name,
                        StringComparison.OrdinalIgnoreCase
                    ));

                if (destinationProperty == null || !destinationProperty.CanWrite) continue;

                var sourceValue = sourceProperty.GetValue(source);
                if (sourceValue == null) continue;

                if (NeedConversion(sourceProperty.PropertyType, destinationProperty.PropertyType))
                {
                    sourceValue = SmartConvert(sourceValue, destinationProperty.PropertyType);
                }

                destinationProperty.SetValue(destination, sourceValue);
            }

            return destination;
        }

        /// <summary>
        /// 自动转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static object SmartConvert(object value, Type targetType)
        {
            var underlyingTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (underlyingTargetType == typeof(string))
            {
                return value.ToString();
            }

            if (value is DateOnly dateOnly && underlyingTargetType == typeof(DateTime))
            {
                return dateOnly.ToDateTime(TimeOnly.MinValue);
            }
            if (value is DateTime dateTime && underlyingTargetType == typeof(DateOnly))
            {
                return DateOnly.FromDateTime(dateTime);
            }

            try
            {
                return Convert.ChangeType(value, underlyingTargetType);
            }
            catch (InvalidCastException)
            {
                throw new InvalidOperationException(
                    $"Cannot convert {value.GetType()} to {targetType}");
            }
        }

        /// <summary>
        /// 特殊类型转换
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private static bool NeedConversion(Type sourceType, Type targetType)
        {
            var underlyingSource = Nullable.GetUnderlyingType(sourceType) ?? sourceType;
            var underlyingTarget = Nullable.GetUnderlyingType(targetType) ?? targetType;
            return underlyingSource != underlyingTarget;
        }

        /// <summary>
        /// 映射实体列表
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="sourceList">源对象列表</param>
        /// <returns>目标对象列表</returns>
        public static List<TDestination> MapList<TSource, TDestination>(List<TSource> sourceList)
            where TDestination : new()
        {
            return sourceList?.Select(Map<TSource, TDestination>).ToList();
        }
    }
}
