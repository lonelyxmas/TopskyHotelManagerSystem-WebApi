using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EOM.TSHotelManagement.Common
{
    public static class EntityMapper
    {
        /// <summary>
        /// 映射单个实体
        /// </summary>
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

                if (sourceValue is DateOnly dateOnlyValue &&
                    destinationProperty.PropertyType == typeof(DateTime?))
                {
                    if (dateOnlyValue == DateOnly.MinValue || dateOnlyValue == new DateOnly(1900, 1, 1))
                    {
                        destinationProperty.SetValue(destination, null);
                    }
                    else
                    {
                        destinationProperty.SetValue(destination, dateOnlyValue.ToDateTime(TimeOnly.MinValue));
                    }
                    continue;
                }

                if (sourceValue == null)
                {
                    if (destinationProperty.PropertyType.IsValueType &&
                        Nullable.GetUnderlyingType(destinationProperty.PropertyType) != null)
                    {
                        destinationProperty.SetValue(destination, null);
                    }
                    continue;
                }

                if (NeedConversion(sourceProperty.PropertyType, destinationProperty.PropertyType))
                {
                    sourceValue = SmartConvert(sourceValue, destinationProperty.PropertyType);
                }

                destinationProperty.SetValue(destination, sourceValue);
            }

            return destination;
        }

        /// <summary>
        /// 智能类型转换
        /// </summary>
        private static object SmartConvert(object value, Type targetType)
        {
            if (value is DateOnly dateOnly)
            {
                return HandleDateOnlyConversion(dateOnly, targetType);
            }

            if (value is DateTime dateTime)
            {
                return HandleDateTimeConversion(dateTime, targetType);
            }

            if (value is string dateString)
            {
                return HandleStringConversion(dateString, targetType);
            }

            if (IsMinValue(value))
            {
                return ConvertMinValueToNull(value, targetType);
            }

            try
            {
                return Convert.ChangeType(value, targetType);
            }
            catch (InvalidCastException)
            {
                var underlyingType = Nullable.GetUnderlyingType(targetType);
                if (underlyingType != null)
                {
                    try
                    {
                        return Convert.ChangeType(value, underlyingType);
                    }
                    catch
                    {

                    }
                }

                throw new InvalidOperationException(
                    $"Cannot convert {value.GetType()} to {targetType}");
            }
        }

        /// <summary>
        /// 检查是否为最小值
        /// </summary>
        private static bool IsMinValue(object value)
        {
            return value switch
            {
                DateTime dt => dt == DateTime.MinValue || dt == new DateTime(1900, 1, 1),
                DateOnly d => d == DateOnly.MinValue || d == new DateOnly(1900, 1, 1),
                DateTimeOffset dto => dto == DateTimeOffset.MinValue || dto == new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero),
                _ => false
            };
        }

        /// <summary>
        /// 将最小值转换为空值
        /// </summary>
        private static object ConvertMinValueToNull(object value, Type targetType)
        {
            if (Nullable.GetUnderlyingType(targetType) != null)
            {
                return null;
            }

            if (targetType == typeof(string))
            {
                return string.Empty;
            }

            return value;
        }

        /// <summary>
        /// 处理 DateOnly 类型转换
        /// </summary>
        private static object HandleDateOnlyConversion(DateOnly dateOnly, Type targetType)
        {
            if (IsMinValue(dateOnly))
            {
                return ConvertMinValueToNull(dateOnly, targetType);
            }

            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            switch (underlyingType.Name)
            {
                case nameof(DateTime):
                    return dateOnly.ToDateTime(TimeOnly.MinValue);

                case nameof(DateTimeOffset):
                    return new DateTimeOffset(dateOnly.ToDateTime(TimeOnly.MinValue));

                case nameof(String):
                    return dateOnly.ToString("yyyy-MM-dd");

                case nameof(DateOnly):
                    return dateOnly;

                default:
                    throw new InvalidCastException($"Unsupported DateOnly conversion to {targetType}");
            }
        }

        /// <summary>
        /// 处理 DateTime 类型转换
        /// </summary>
        private static object HandleDateTimeConversion(DateTime dateTime, Type targetType)
        {
            if (IsMinValue(dateTime))
            {
                return ConvertMinValueToNull(dateTime, targetType);
            }

            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            switch (underlyingType.Name)
            {
                case nameof(DateOnly):
                    return DateOnly.FromDateTime(dateTime);

                case nameof(DateTimeOffset):
                    return new DateTimeOffset(dateTime);

                case nameof(String):
                    return dateTime.ToString("yyyy-MM-dd HH:mm:ss");

                case nameof(DateTime):
                    return dateTime;

                default:
                    return dateTime;
            }
        }

        /// <summary>
        /// 处理字符串日期转换
        /// </summary>
        private static object HandleStringConversion(string dateString, Type targetType)
        {
            if (DateTime.TryParse(dateString, out DateTime dt))
            {
                return HandleDateTimeConversion(dt, targetType);
            }

            if (DateOnly.TryParse(dateString, out DateOnly d))
            {
                return HandleDateOnlyConversion(d, targetType);
            }

            if (string.IsNullOrWhiteSpace(dateString))
            {
                return ConvertMinValueToNull(DateTime.MinValue, targetType);
            }

            throw new FormatException($"Invalid date string: {dateString}");
        }

        /// <summary>
        /// 判断是否需要类型转换
        /// </summary>
        private static bool NeedConversion(Type sourceType, Type targetType)
        {
            var underlyingSource = Nullable.GetUnderlyingType(sourceType) ?? sourceType;
            var underlyingTarget = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (underlyingSource == underlyingTarget) return false;

            return true;
        }

        /// <summary>
        /// 映射实体列表
        /// </summary>
        public static List<TDestination> MapList<TSource, TDestination>(List<TSource> sourceList)
            where TDestination : new()
        {
            return sourceList?.Select(Map<TSource, TDestination>).ToList();
        }
    }
}