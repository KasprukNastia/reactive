namespace AuthAPI.Tokens
{
    public interface ITokenGenerator
    {
        string GenerateToken(string userName);
    }
}
