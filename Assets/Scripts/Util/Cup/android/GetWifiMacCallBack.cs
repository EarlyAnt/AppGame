using UnityEngine;

namespace AppGame.Util
{
    public class GetWifiMacCallBack : AndroidJavaProxy
    {
        public GetWifiMacCallBack() : base("com.bowhead.hank.ble.WifiUtils$GetWifiMacCallBack"){}

        public void onValue(string showMac){
           
            if(mGetMacValue != null){
                mGetMacValue(showMac);
            }
             
        }

        private GetMacValue mGetMacValue;
        public void setCallBack(GetMacValue mGetMacValue){
            this.mGetMacValue = mGetMacValue;
        }
    }

    public delegate void GetMacValue(string mac);
}