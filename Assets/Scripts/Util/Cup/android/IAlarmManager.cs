using System;

namespace Cup.Utils.android
{
    public interface IAlarmManager
    {
        //void init();
        void snooze();
        void dismiss();

        void setAlarmListener(Action<string> alarmAlert,Action<string> alarmMissed);

        void replaceAlarmList(string alarmInfo, Action success,Action error);
        string GetCurrentAlarm();
        void SetCurrentAlarm(string alarm);
    }
}