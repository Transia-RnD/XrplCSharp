using System;
using System.Collections.Generic;
using System.Linq;

namespace Xrpl.Client.Extensions;

public static class EmumExtensions
{
    public static IEnumerable<T> DecodeFlags<T>(this T e) where T : Enum
    {
        if (e is null) return Array.Empty<T>();
        var type = e.GetType();
        var income_int_val = Convert.ToUInt32(e);
        var values = new List<T>();
        foreach (var obj in Enum.GetValues(type).Cast<T>())
        {
            var flag_int_val = Convert.ToUInt32(obj);
            if (income_int_val != 0 && (income_int_val & flag_int_val) == flag_int_val)
                values.Add(obj);
        }

        return values;
    }
    public static string FlagsValues<T>(this T Flags) where T : Enum
    {
        if (Flags is null)
            return string.Empty;
        var values = Flags.DecodeFlags().ToArray();
        return values.Length switch
        {
            0 => Flags.ToString(),
            1 => values[0].ToString(),
            _ => string.Join(" | ", values)
        };
    }
}