namespace Pangea.Shared.Attributes.Authorization.Contracts
{
    public interface IUserInfo
    {
        string? UserName { get; }
        string? SecurityStamp { get; }
        string? UserId { get; }
        string? ClientIp { get; }
        string? ClientDevice { get; }
        IEnumerable<string> Claims { get; }

        bool HasClaim(string claim);
        bool IsUserAuthenticated();
    }
}
