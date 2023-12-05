using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Theater.Common.Enums;

namespace Theater.Common.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Получить значение атрибута <see cref="DisplayAttribute.Name"/>
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public static string GetEnumDisplayName(this Enum property)
    {
        var enumDisplayName = property.GetType()
            .GetMember(property.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            ?.GetName();

        return enumDisplayName;
    }

    /// <summary>
    /// Преобразует строку <paramref name="input"/> в <typeparamref name="TEnum"/>
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="input">Строка</param>
    /// <exception cref="ArgumentException"></exception>
    public static TEnum ConvertStringToEnum<TEnum>(this string input) where TEnum : struct, Enum
    {
        if(string.IsNullOrWhiteSpace(input))
            return default;

        var values = input.Split(' ').ToList();

        var result = new List<TEnum>(values.Capacity);

        foreach (var value in values)
        {
            if (!Enum.TryParse<TEnum>(value, ignoreCase:true, out var enumValue))
                throw new ArgumentException($"Передано некорректное значение для {typeof(TEnum).Name}");

            result.Add(enumValue);
        }

        return result.Aggregate<TEnum, TEnum>(
            seed: default,
            (current, value) => (TEnum)(object)((int)(object)current | (int)(object)value));
    }
}