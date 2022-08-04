using System;
using System.Collections.Generic;
using System.Linq;

namespace Xrpl.Client.Extensions;

public static class EmumExtensions
{
    public static IEnumerable<T> DecodeFlags<T>(this T e) where T : Enum
    {
        var genericType = typeof(T);
        if (!genericType.IsEnum) return default;
        var income_int_val = Convert.ToUInt32(e);
        var values = new List<T>();
        foreach (var obj in Enum.GetValues(typeof(T)).Cast<T>())
        {
            var flag_int_val = Convert.ToUInt32(obj);
            if (income_int_val != 0 && (income_int_val & flag_int_val) == flag_int_val)
                values.Add(obj);
        }

        return values;
    }
    public static string FlagsValues<T>(this T Flags) where T : Enum =>
        Flags is { } flag && flag.DecodeFlags().ToArray() is { Length: > 0 } flags 
            ? flags.Length > 1 
                ? string.Join(" | ", flags) : flags[0].ToString()
            : "0";
}