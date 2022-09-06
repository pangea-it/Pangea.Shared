using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Shared.Attributes.Claims
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class DescriptionAttribute : Attribute
    {
        public string NameDescription { get; set; }
        public DescriptionAttribute(string nameDescriptions)
        {
            NameDescription = nameDescriptions;
        }
    }
}
