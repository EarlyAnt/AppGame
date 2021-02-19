using System;
using UnityEngine;

namespace CupSdk.BaseSdk.Wifi
{
    public class ScanCallback : AndroidJavaProxy
    {
        public ScanCallback() : base("com.bowhead.sheldon.wifi.WifiManagerWrap$ScanCallback"){}

        public void onScanResults(AndroidJavaObject results){
            if(mResults != null)
                mResults(results);
        }

        public void onScanFailed(){
            if(mFailed != null)
                mFailed(false);
        }

        Action<AndroidJavaObject> mResults;
        Action<bool> mFailed;
        public void addListener(Action<AndroidJavaObject> results,Action<bool> failed){
            mResults =  results;
            mFailed = failed;
        }
    }
}