using Microsoft.AspNetCore.Mvc;
using Pangea.Shared.Attributes.Claims;
using System.Reflection;

namespace Pangea.Shared.Attributes.Readers
{
    public static class ClaimAttributeReader
    {
        public static IEnumerable<(string group, string claim, string description)> GetDescriptionAttributes(params Assembly[] list)
        {
            var listOfAttributes = new List<(string, string, string)>();
            foreach (var item in list)
            {
                var methods = item.GetTypes()
                    .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods())
                    .Where(m => m.GetCustomAttributes(typeof(DescriptionAttribute), true).Any());
                foreach (var item2 in methods)
                {
                    var attr = item2.GetCustomAttribute<DescriptionAttribute>();
                    var group = item2.DeclaringType.Name.Replace("Controller", "");
                    var claim = $"{group}/{item2.Name}";
                    listOfAttributes.Add((group, claim, attr.Description));
                }
            }
            return listOfAttributes;
        }
    }
}
