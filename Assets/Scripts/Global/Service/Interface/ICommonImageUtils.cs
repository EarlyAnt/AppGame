using UnityEngine;

namespace AppGame.Global
{
    public interface ICommonImageUtils
    {
        void Initialize();
        void LoadCommonImages();
        Sprite GetAvatar(string avatarName);
    }
}