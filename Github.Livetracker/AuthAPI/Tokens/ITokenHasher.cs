namespace AuthAPI.Tokens
{
    public interface ITokenHasher
    {
        public string HashToken(string token);
    }
}
