using AppGame.Global;

namespace AppGame.Data.Remote
{
    public interface IUrlProvider
    {
        string GetVerifyCodeUrl();
        string LoginUrl();

        string GetTokenUrl(string child_sn);
        string GetRegisterUserUrl(string prod_name, string cup_hw_sn);
        string ItemDataUrl(string child_sn);
        string GetBasicDataUrl(string child_sn);
        string GetOriginDataUrl(string child_sn, string device_type);
        string GetGameDataUrl(string child_sn);
        string PutGameDataUrl(string child_sn);
        string GetUpdateInfo(string cup_hw_sn);
    }
}