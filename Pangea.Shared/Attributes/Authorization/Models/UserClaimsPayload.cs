namespace Pangea.Shared.Attributes.Authorization.Models
{
    public class UserClaimsPayload
    {
        public int UserId { get; set; }
        public List<ClaimPayload> Claims { get; set; } = new();
    }
}
