using System;

namespace BanBrick.Utils.Extensions
{
    public static class StringExtensions
    {
        public static TEnum? ToEnum<TEnum>(this string value) where TEnum : struct, IConvertible
        {
            return EnumConvertor.ToEnum<TEnum>(value);
        }

        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value[0].ToString().ToLower() + value.Substring(1);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
