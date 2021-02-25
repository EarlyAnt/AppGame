using Cup.Utils.android;
using System;
using UnityEngine;

namespace AppGame.Util
{
    public class CupBuild
    {
        private static string CUP_SN = "";
        private static event WifiReceiverListenerCallback WifiReceiverListenerCallbackEvent = delegate (int status) { };


#if !UNITY_EDITOR
        private static readonly AndroidJavaClass CupBaseInfoManager = new AndroidJavaClass("com.bowhead.hank.utils.CupBaseInfoManager");
        private static readonly AndroidJavaClass nativeUtils = new AndroidJavaClass("com.bowhead.hank.utils.Utils");
        private static AndroidJavaObject nativeWifiUtils;
        private static WifiReceiverListener mWifiReceiverListener;
#endif

        public static string getCupSn()
        {
#if UNITY_EDITOR
            CUP_SN = "";//ServiceConfig.defaultCupHwSn;
#else
            if(CUP_SN == ""){
                CUP_SN = CupBaseInfoManager.CallStatic<string>("getCupSN").Trim();

            }
#endif
            return CUP_SN;
        }

        private static string getRandom()
        {
            return UnityEngine.Random.Range(1000000000, 9999999999).ToString();
        }

        public static void getCupHwMac(GetMacValue mGetMacValue)
        {
#if UNITY_EDITOR
            mGetMacValue("02:00:00:00:00:00");
#else
        if(nativeWifiUtils == null){
           nativeWifiUtils = new AndroidJavaObject("com.bowhead.hank.ble.WifiUtils",AndroidContextHolder.GetAndroidContext());
        }

        GetWifiMacCallBack callBack = new GetWifiMacCallBack();
        callBack.setCallBack((mac)=>{
            mGetMacValue(mac);
        });
                          
        nativeWifiUtils.Call("getWifiMac",callBack);

#endif
        }

        public static void getCanShowMacId(GetMacValue mGetMacValue)
        {
#if UNITY_EDITOR
            mGetMacValue("02:00:00:00:00:00");
#else
        if(nativeWifiUtils == null){
           nativeWifiUtils = new AndroidJavaObject("com.bowhead.hank.ble.WifiUtils",AndroidContextHolder.GetAndroidContext());
        }

        GetWifiMacCallBack callBack = new GetWifiMacCallBack();
        callBack.setCallBack((mac)=>{
            Loom.QueueOnMainThread(() =>
                {
                    mGetMacValue(mac);
                });
        });
                          
        nativeWifiUtils.Call("getShowWifiMac",callBack);

#endif
        }


        public static string getFizzVersionNane()
        {
# if UNITY_EDITOR
            return "0.0.0";
#else
            return CupBaseInfoManager.CallStatic<string>("getFizzVersionName",AndroidContextHolder.GetAndroidContext());
#endif
        }

        public static string getAppVersionName()
        {
# if UNITY_EDITOR
            return "0.0.0";
#else
            return CupBaseInfoManager.CallStatic<string>("getAppVersionNane",AndroidContextHolder.GetAndroidContext());
#endif
        }


        public static void registerWifiConnectListener(WifiReceiverListenerCallback callback)
        {
            WifiReceiverListenerCallbackEvent += callback;
#if UNITY_EDITOR

#else
        if(mWifiReceiverListener == null){
            mWifiReceiverListener = new WifiReceiverListener();
            mWifiReceiverListener.setCallback((status)=>{
                    WifiReceiverListenerCallbackEvent(status);
            });
            nativeUtils.CallStatic("registerWifiConnectListener",AndroidContextHolder.GetAndroidContext(),mWifiReceiverListener);

        }
#endif
        }

        public static void unRegisterWifiConnectListener(WifiReceiverListenerCallback callback)
        {
            WifiReceiverListenerCallbackEvent -= callback;
        }


    }
}