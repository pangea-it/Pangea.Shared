using Newtonsoft.Json;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Pangea.Shared.Extensions.General
{
    public static class PrimitiveExtensions
    {
        public static string ToJson<T>(this T input)
        {
            return JsonConvert.SerializeObject(input);
        }

        public static T FromJson<T>(this string input)
        {
            return JsonConvert.DeserializeObject<T>(input)!;
        }

        public static bool IsNumeric(this Type input)
        {
            return
            (
                input == typeof(short) ||
                input == typeof(int) ||
                input == typeof(long) ||
                input == typeof(float) ||
                input == typeof(double) ||
                input == typeof(decimal) ||
                input == typeof(short?) ||
                input == typeof(int?) ||
                input == typeof(long?) ||
                input == typeof(float?) ||
                input == typeof(double?) ||
                input == typeof(decimal?)
            );
        }

        public static bool IsDecimal(this Type input)
        {
            return
            (
                input == typeof(decimal) ||
                input == typeof(decimal?)
            );
        }

        public static bool IsDateTime(this Type input)
        {
            return
            (
                input == typeof(DateTime) ||
                input == typeof(DateTime?)
            );
        }

        public static int TryConvertToInt(this string? input)
        {
            _ = int.TryParse(input, out int result);

            return result;
        }

        public static int TryConvertToInt(this object input)
        {
            _ = int.TryParse(input.ToString(), out int result);

            return result;
        }

        public static long TryConvertToLong(this string? input)
        {
            _ = long.TryParse(input, out long result);

            return result;
        }

        public static bool HasValue(this string? input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        public static string? TrimAll(this string? input)
        {
            return input?.Trim(new[] { '\t', '\r', '\n', ' ', '\0' });
        }

        public static IEnumerable<KeyValuePair<string, string>> ToKeyValues(this object input)
        {
            var result = new List<KeyValuePair<string, string>>();

            PropertyInfo[] props = input.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var prop in props)
            {
                result.Add
                (
                    new KeyValuePair<string, string>
                    (
                        char.ToLowerInvariant(prop.Name[0]) + prop.Name[1..], prop.GetValue(input)?.ToString()!
                    )
                );
            }

            return result;
        }

        public static T? ConvertTo<T>(this object source, T? target = null, bool ignoreNulls = false) where T : class, new()
        {
            if (source != null)
            {
                target ??= Activator.CreateInstance<T>();

                foreach (PropertyInfo pf in typeof(T).GetProperties())
                {
                    if (pf.GetSetMethod() != null)
                    {
                        MethodInfo smi = source.GetType().GetMethod($"get_{pf.Name}", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)!;
                        if (smi != null)
                        {
                            object srcPropValue = smi.Invoke(source, null)!;
                            if (srcPropValue != null || ignoreNulls == false)
                            {
                                MethodInfo tmi = typeof(T).GetMethod($"set_{pf.Name}", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)!;
                                tmi.Invoke(target, new object[] { srcPropValue! });
                            }
                        }
                    }
                }

                return target;
            }

            return null;
        }

        internal static bool In<T, K>(this T item, params K[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            K itemX = (K)Convert.ChangeType(item, typeof(K))!;

            if (itemX == null) return false;

            return items.Contains(itemX);
        }

        public static string FindAndReplace(this string input, string searchPattern, Dictionary<string, string> replacementValues)
        {
            var rx = new Regex(searchPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            if (rx.IsMatch(input))
            {
                return rx.Replace
                (
                    input,
                    (Match m) =>
                    {
                        return replacementValues.ContainsKey(m.Value)
                               ? replacementValues[m.Value]
                               : m.Value;
                    }
                );
            }

            return input;
        }
    }
}
