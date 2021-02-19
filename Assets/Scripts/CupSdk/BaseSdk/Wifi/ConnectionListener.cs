using System;
using UnityEngine;

namespace CupSdk.BaseSdk.Wifi
{
    public class ConnectionListener : AndroidJavaProxy
    {
        public ConnectionListener() : base("com.bowhead.sheldon.wifi.WifiManagerWrap$ConnectionListener"){}

        public void onConnected(AndroidJavaObject wifiInfo){
            if(mWifiInfo != null){
                 Loom.QueueOnMainThread(()=>{
                    mWifiInfo(wifiInfo);
                });
                
            }
                
        }
        public void onDisConnected(string SSID){
            if(mDisconnect != null){
                 Loom.QueueOnMainThread(()=>{
                    mDisconnect(SSID);
                });
                
            }
                
        }

        public void onAuthenticating(string SSID)
        {
            if (mAuthenticating != null)
            {
                 Loom.QueueOnMainThread(()=>{
                    mAuthenticating(SSID);
                });
                
            }
        }

        public void onAuthError(string SSID)
        {
            if (mAuthError != null)
            {
                 Loom.QueueOnMainThread(()=>{
                    mAuthError(SSID);
                });
                
            }
        }

        private Action<AndroidJavaObject> mWifiInfo;
        private Action<string> mDisconnect;

        private Action<string> mAuthenticating;

        private Action<string> mAuthError;
        public void addCallback(Action<AndroidJavaObject> wifiInfo, Action<string> disconnect,
        Action<string> authenticating, Action<string> authError)
        {
            mWifiInfo = wifiInfo;
            mDisconnect = disconnect;
            mAuthenticating = authenticating;
            mAuthError = authError;
        }

    }
}