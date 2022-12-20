using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Shared.Attributes.Authorization.Models
{
    public class ClaimPayload
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public string GroupName { get; set; } = string.Empty;
    }
}
