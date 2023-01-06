using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Theater.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumDisplayName(this Enum property)
        {
            var enumDisplayName = property.GetType()
                .GetMember(property.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                ?.GetName();

            return enumDisplayName;
        }
    }
}