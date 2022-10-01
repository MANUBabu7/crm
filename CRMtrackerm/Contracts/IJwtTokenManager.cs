namespace CrmTracker.Contracts
{
    public interface IJwtTokenManager
    {
        string GenerateToken(string username,string role);
    }
}