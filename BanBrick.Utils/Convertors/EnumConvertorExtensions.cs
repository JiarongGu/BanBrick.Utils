using System;

namespace BanBrick.Utils.Convertors
{
    public static class EnumConvertorExtensions
    {
        public static TEnum? ToEnum<TEnum>(this string value) where TEnum : struct, IConvertible
        {
            return EnumConvertor.ToEnum<TEnum>(value);
        }

        public static string ToText<TEnum>(this TEnum value) where TEnum : struct, IConvertible
        {
            return EnumConvertor.ToText(value);
        }
    }
}
