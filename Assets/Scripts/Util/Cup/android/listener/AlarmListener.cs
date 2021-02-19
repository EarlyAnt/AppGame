using System;
using UnityEngine;

namespace Cup.Utils.android
{
    public class AlarmListener : AndroidJavaProxy
    {
        public AlarmListener() : base("com.bowhead.sheldon.alarm.AlarmManager$AlarmListener"){}

        public void onAlarmAlert(AndroidJavaObject alarmInstance){
            if(mAlarmAlert != null){
                AlarmInstance alarm = new AlarmInstance(alarmInstance);
                mAlarmAlert(alarm);
            }
        }

        public void onAlarmMissed(AndroidJavaObject alarmInstance){
            if(mAlarmMissed != null){
                AlarmInstance alarm = new AlarmInstance(alarmInstance);
                mAlarmMissed(alarm);
            }
        }

        private Action<AlarmInstance> mAlarmAlert;
        private Action<AlarmInstance> mAlarmMissed;
        public void setListener(Action<AlarmInstance> alarmAlert,Action<AlarmInstance> alarmMissed){
            mAlarmAlert = alarmAlert;
            mAlarmMissed = alarmMissed;
        }

    }

    public class AlarmInstance
    {
        AndroidJavaObject mAlarmInstance;
        public AlarmInstance(AndroidJavaObject alarmInstance){
            mAlarmInstance = alarmInstance;
        }

        public string getAction(){
            return mAlarmInstance.Call<string>("getAction");
        }
    }
}