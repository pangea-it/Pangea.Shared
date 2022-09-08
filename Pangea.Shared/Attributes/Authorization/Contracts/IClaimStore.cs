namespace Pangea.Shared.Attributes.Authorization.Contracts
{
    public interface IClaimStore
    {
        Task<IEnumerable<string>> GetUserClaims(int userId, string securityStamp);
    }
}
