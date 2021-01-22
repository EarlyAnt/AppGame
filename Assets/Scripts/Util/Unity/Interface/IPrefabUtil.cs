using System;
using UnityEngine;

namespace AppGame.Util
{
    public interface IPrefabUtil
    {
        GameObject CreateGameObject(string path, string prefabName);

        T GetGameObject<T>(string path, string objectName) where T : UnityEngine.Object;
    }
}

