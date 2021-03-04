using System.IO;
using UnityEngine;

namespace AppGame.Data.Local
{
    public class LocalPathHelper : ILocalPathHelper
    {
        public string GetStreamingFilePath(string filename)
        {
            string path = "";
            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer ||
                Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                path = Application.dataPath + Path.DirectorySeparatorChar + "StreamingAssets" + Path.DirectorySeparatorChar + filename;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                path = Application.dataPath + "/Raw/" + filename;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                path = "jar:file://" + Application.dataPath + "!/assets/" + filename;
            }
            else
            {
                path = Application.dataPath + Path.DirectorySeparatorChar + "config" + Path.DirectorySeparatorChar + filename;
            }
            return path;
        }

        public string GetPersistentFilePath(string filename)
        {
            string filepath = Application.persistentDataPath + Path.DirectorySeparatorChar + filename;
#if UNITY_IPHONE
                UnityEngine.iOS.Device.SetNoBackupFlag(filepath);
#endif
            return filepath;
        }
    }
}