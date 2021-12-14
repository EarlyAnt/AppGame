using UnityEngine;

namespace AppGame.Global
{
    public interface ICommonImageUtils
    {
        void LoadCommonImages();
        string GetAvatarFileName(string avatarName);
    }
}