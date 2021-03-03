using Cup.Utils.android;
using UnityEngine;

namespace AppGame.Data.Local
{
    public class NativeDataManager : INativeDataManager
    {
        public void saveToken(string token)
        {
            Debug.Log("save token : " + token);
            AndroidJavaObject mainActiviry = AndroidContextHolder.GetAndroidContext();

            mainActiviry.Call<bool>("saveToken", token);
        }
    }
}