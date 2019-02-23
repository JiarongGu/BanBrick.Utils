using System;

namespace BanBrick.Utils.Convertors
{
    public class EnumTextAttribute : Attribute
    {
        public EnumTextAttribute(string text) {
            Text = text;
        }

        public string Text { get; }
    }
}
