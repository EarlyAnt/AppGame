namespace AppGame.Data.Local
{
    public interface ILocalCupAgent
    {
        ILocalDataManager LocalDataManager { get; set; }
        void SaveCupToken(string cupSN, string token);
        string GetCupToken(string cupSN);
        string GetCupToken();

        void SaveCupID(string cupSN, string cupID);
        string GetCupID(string cupSN);
    }
}