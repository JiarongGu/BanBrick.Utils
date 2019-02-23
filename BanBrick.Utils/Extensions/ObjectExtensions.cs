using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BanBrick.Utils.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNumericType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static Type GetGenericTypeDefinitionOrDefault(this Type type)
        {
            if (type == null || !type.IsGenericType)
                return null;

            return type.GetGenericTypeDefinition();
        }

        public static bool IsImplementedInterface(this Type type, Type implementedType)
        {
            var isAssignableFrom = type
                .GetInterfaces()
                .Select(x => x.GetGenericTypeDefinitionOrDefault() ?? x)
                .Contains(implementedType);

            return type.IsClass && !type.IsAbstract && !type.IsInterface && isAssignableFrom;
        }

        public static IEnumerable<Type> GetMatchedInterfaces(this Type type, Type implementedType)
        {
            return type.GetInterfaces().Where(x => x.GetGenericTypeDefinitionOrDefault() == implementedType || x == implementedType);
        }

        /// <summary>
        /// Parse object using JsonConvert, can convert anonymous to typed object
        /// </summary>
        public static T Parse<T>(this object value) where T : class, new()
        {
            if (value == null)
                return null;

            var json = JsonConvert.SerializeObject(value);
            var parsedObject = JsonConvert.DeserializeObject<T>(json);
            return parsedObject;
        }
    }
}
