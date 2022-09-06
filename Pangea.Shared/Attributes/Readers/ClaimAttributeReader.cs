using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Pangea.Shared.Attributes.Readers
{
    public static class ClaimAttributeReader
    {
        public static Dictionary<string, string> GetClaimsFromAttributes<T>(string propertyName, params Assembly[] list) where T : Attribute
        {
            var dict = new Dictionary<string, string>();
            var res = GetCustomAttributes<T>(list);
            foreach (var item in res)
            {
                var prop = typeof(T).GetProperty(propertyName).GetValue(item);
                var keyVals = prop.ToString().Split(";");
                foreach (var item2 in keyVals)
                {
                    var keyVal = item2.Split(" ");
                    var key = keyVal[0].Trim();
                    var val = keyVal[1].Trim();
                    if (!dict.ContainsKey(key))
                        dict[key] = val;
                }
            }
            return dict;
        }
        private static IEnumerable<T> GetCustomAttributes<T>(params Assembly[] list) where T : Attribute
        {

            var lists = new List<T>();
            foreach (var item in list)
            {
                lists.AddRange(GetAssemblyCustumAttributes<T>(item));
            }
            return lists;
        }



        private static IEnumerable<T> GetAssemblyCustumAttributes<T>(Assembly assembly) where T : Attribute
        {
            var attributes = assembly.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(T), true).Any())
                .Select(x => x.GetCustomAttribute(typeof(T)))
                .Select(y => (T)y);
            return attributes;
        }
    }
}
