using System;
using System.Linq;

namespace BanBrick.Utils.Enum
{
    public static class EnumConvertor
    {
        public static TEnum? ToEnum<TEnum>(string value) where TEnum : struct, IConvertible
        {
            if(Enum.TryParse<TEnum>(value, out var enumValue))
                return enumValue;
            return null;
        }

        public static TEnum? ToEnumFromText<TEnum>(string value) where TEnum: struct, IConvertible
        {
            var enumValues = Enum.GetValues(typeof(TEnum));

            foreach (TEnum enumValue in enumValues)
            {
                if (ToText(enumValue) == value)
                    return enumValue;
            }

            return null;
        }

        public static string ToText<TEnum>(TEnum e) where TEnum : struct, IConvertible
        {
            var enumType = e.GetType();
            var enumName = Enum.GetName(enumType, e);
            var enumAttribute = enumType.GetField(enumName).GetCustomAttributes(false).OfType<EnumTextAttribute>().FirstOrDefault();

            return (enumAttribute != null) ? enumAttribute.Text : e.ToString();
        }
    }
}
