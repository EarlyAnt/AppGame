using AppGame.Global;

namespace AppGame.Data.Remote
{
    public interface IUrlProvider
    {
        string GetTokenUrl(string child_sn);
        string GetRegisterUserUrl(string prod_name, string cup_hw_sn);
        string GetBasicDataUrl(string child_sn);
        string GetGameDataUrl(string child_sn);
        string PutGameDataUrl(string child_sn);
    }
}