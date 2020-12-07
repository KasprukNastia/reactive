namespace AuthAPI.AppCode.Interfaces
{
    public interface ITokenHasher
    {
        public string HashToken(string token);
    }
}
