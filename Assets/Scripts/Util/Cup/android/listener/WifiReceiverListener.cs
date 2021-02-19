using System;
using UnityEngine;

namespace AppGame.Util
{
    public class WifiReceiverListener : AndroidJavaProxy
    {
        public WifiReceiverListener() : base("com.bowhead.hank.network.broadcast.WifiReceiver$Listener") { }

        public void onConnectChange(int state)
        {
            if (mStateCallback != null)
            {
                Loom.QueueOnMainThread(() =>
                {
                    if (Application.internetReachability == NetworkReachability.NotReachable)
                    {
                        mStateCallback(state);
                    }
                });
            }
        }

        private WifiReceiverListenerCallback mStateCallback;

        public void setCallback(WifiReceiverListenerCallback stateCallback)
        {
            this.mStateCallback = stateCallback;
        }
    }

    public delegate void WifiReceiverListenerCallback(int status);
}