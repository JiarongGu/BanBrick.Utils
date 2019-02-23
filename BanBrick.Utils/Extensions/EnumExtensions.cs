using System;

namespace BanBrick.Utils.Extensions
{
    public static class EnumExtensions
    {
        public static string ToText<TEnum>(this TEnum value) where TEnum : struct, IConvertible
        {
            return EnumConvertor.ToText(value);
        }
    }
}
