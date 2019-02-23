using System;

namespace BanBrick.Utils.Enum
{
    public class EnumTextAttribute : Attribute
    {
        public EnumTextAttribute(string text) {
            Text = text;
        }

        public string Text { get; }
    }
}
