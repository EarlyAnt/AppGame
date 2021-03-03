namespace AppGame.Data.Local
{
    public interface ITokenManager
    {
        void SaveToken(string token);
        string GetToken();
    }
}