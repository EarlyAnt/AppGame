using System;
using UnityEngine;

namespace Cup.Utils.android
{
    public class SetAlarmCallback : AndroidJavaProxy
    {
        public SetAlarmCallback() : base("com.bowhead.sheldon.alarm.AlarmManager$SetAlarmCallback"){}

        public void onSucceed(){
            if(mSuccess != null){
                mSuccess();
            }
        }
        public void onError(){
            if(mError != null){
                mError();
            }
        }

        private Action mSuccess;
        private Action mError;

        public void setCallback(Action success,Action error){
            mSuccess = success;
            mError = error;
        }
    }
}