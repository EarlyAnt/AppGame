using AppGame.Global;

namespace AppGame.Data.Remote
{
    public interface IUrlProvider
    {
        string GetCupTokenUrl(string cup_hw_sn);
        string GetRegisterUrl(string prod_name, string cup_hw_sn);
        string GetGameDataUrl(string child_sn, string game_name, string cup_hw_sn);
    }
}