using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace BanBrick.Utils.Internals
{
    internal class PropertyValidation
    {
        public PropertyInfo Property { get; set; }

        public IEnumerable<ValidationAttribute> Validations { get; set; }
    }
}
