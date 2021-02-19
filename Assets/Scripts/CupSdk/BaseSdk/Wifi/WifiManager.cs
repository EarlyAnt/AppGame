using UnityEngine;

namespace CupSdk.BaseSdk.Wifi
{
    public class WifiManager
    {
        private AndroidJavaObject javaWifiManager;

        private static AndroidJavaClass javaWifiManagerclass = new AndroidJavaClass("com.bowhead.sheldon.wifi.WifiManagerWrap");

        private static AndroidJavaClass nativeWifiManager = new AndroidJavaClass("android.net.wifi.WifiManager");

        public WifiManager(AndroidJavaObject wifiManager) {
            javaWifiManager = new AndroidJavaObject("com.bowhead.sheldon.wifi.WifiManagerWrap",wifiManager);
        }

        public bool setWifiEnabled(bool enable){
            return javaWifiManager.Call<bool>("setWifiEnabled",enable);
        }

        public bool isWifiEnabled(){
            return javaWifiManager.Call<bool>("isWifiEnabled");
        }

        public int getWifiState(){
            return javaWifiManager.Call<int>("getWifiState");
        }

        public AndroidJavaObject getConnectionInfo(){
            return javaWifiManager.Call<AndroidJavaObject>("getConnectionInfo");
        }

        public int getRssi(){
            int rssi =  javaWifiManager.Call<AndroidJavaObject>("getConnectionInfo").Call<int>("getRssi");

            return nativeWifiManager.CallStatic<int>("calculateSignalLevel",rssi,4);

        }

        public bool disableNetwork(int netId){
            return javaWifiManager.Call<bool>("disableNetwork",netId);
        }

        public bool disconnect(){
            return javaWifiManager.Call<bool>("disconnect");
        }

        public void acquireWifiLock(){
            javaWifiManager.Call("acquireWifiLock");
        }

        public void releaseWifiLock(){
            javaWifiManager.Call("releaseWifiLock");
        }

        public bool startScan(ScanCallback scanCallback){
            return javaWifiManager.Call<bool>("startScan",scanCallback);
        }

        public static int getSecurity(AndroidJavaObject result){
            return javaWifiManagerclass.CallStatic<int>("getSecurity",result);
        }

        public static int getPskType(AndroidJavaObject result){
            return javaWifiManagerclass.CallStatic<int>("getPskType",result);
        }

        public bool connectWifi(AndroidJavaObject result, string password){
            return javaWifiManager.Call<bool>("connectWifi",result,password);
        }

        public void registerWifiStateListener(WifiStateListener listener){
            javaWifiManager.Call("registerWifiStateListener",listener);
        }

        public void registerConnectionListener(ConnectionListener listener){
            javaWifiManager.Call("registerConnectionListener",listener);
        }

        public void registerReceiver(AndroidJavaObject context){
            javaWifiManager.Call("registerReceiver",context);
        }

        public void unregisterReceiver(AndroidJavaObject context){
            javaWifiManager.Call("unregisterReceiver",context);
        }
    }
}