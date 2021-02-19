using UnityEngine;

namespace Cup.Utils.android
{
    public static class ANRFlagManager
    {
        static AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();

        public static bool ANROccurred()
        {
            try
            {
                if (context != null)
                    return context.Call<bool>("isANROccurred");
                else
                    return false;
            }
            catch (System.Exception ex)
            {
                Debug.LogErrorFormat("<><ANRFlagManager.ANROccurred>Error: {0}", ex.Message);
                return false;
            }
        }

        public static void ClearFlag()
        {
            try
            {
                if (context != null)
                    context.Call("cleanANRFlag");
            }
            catch (System.Exception ex)
            {
                Debug.LogErrorFormat("<><ANRFlagManager.ClearFlag>Error: {0}", ex.Message);
            }
        }
    }
}
