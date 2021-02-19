using Cup.Utils.android;
using CupSdk.Screen;
using UnityEngine;

namespace Cup.Utils.Screen
{
    public class ScreenManager
    {
        private static event ScreenStatus ScreenStatusEvent = delegate (string status) { };
        private static ScreenStatusListener listener;

        private static void start()
        {
            if (listener == null)
            {
                listener = new ScreenStatusListener();
                listener.setListener(onScreenStatus);
                AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();
                ScreenUtils.StartScreenListener(context, listener);
            }
        }



        private static void onScreenStatus(string status)
        {
            Loom.QueueOnMainThread(() =>
            {
                ScreenStatusEvent(status);
            });
        }

        public static void addListener(ScreenStatus listener)
        {
            start();
            ScreenStatusEvent += listener;
        }

        public static void removeListener(ScreenStatus listener)
        {
            ScreenStatusEvent -= listener;
        }

        public static void turnOnScreen()
        {
            AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();
            ScreenUtils.TurnOnScreen(context);
        }
    }
}