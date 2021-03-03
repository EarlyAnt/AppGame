namespace AppGame.Data.Local
{
    public interface ILocalTokenAgent
    {
        void SaveToken(string token);
        string GetToken();
    }
}