using System;
using UnityEngine;

namespace AppGame.Util
{
    public class PrefabUtil : IPrefabUtil
    {
        private const string RESOURCES_PATH = "Prefabs";

        public GameObject CreateGameObject(string path, string prefabName)
        {
            string fullPath = string.Format("{0}/{1}/{2}", RESOURCES_PATH, path, prefabName);
            UnityEngine.Object prefab = Resources.Load(fullPath);
            if (prefab != null)
            {
                GameObject gameObject = GameObject.Instantiate(prefab) as GameObject;
                return gameObject;
            }
            else
            {
                Debug.LogErrorFormat("<><PrefabUtil.CreateGameObject>Error: can not find object, path: {0}", fullPath);
                return null;
            }
        }

        public T GetGameObject<T>(string path, string objectName) where T : UnityEngine.Object
        {
            string fullPath = string.Format("{0}/{1}/{2}", RESOURCES_PATH, path, objectName);
            T gameObject = Resources.Load<T>(fullPath);
            if (gameObject != null)
            {
                return gameObject;
            }
            else
            {
                throw new ArgumentException("<><PrefabUtil.GetGameObject>Error: can not find object, path: {0}", fullPath);
            }
        }
    }
}

