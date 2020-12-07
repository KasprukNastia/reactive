namespace AuthAPI.AppCode.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(string userName);
    }
}
