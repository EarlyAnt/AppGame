using UnityEngine;

namespace Cup.Utils.android
{
    public interface IFlurryUtils
    {
        void init ();

        void setUserId(string userId);

        void onEvent(string eventId);

        void onEvent(string eventId, string paramValue);
    
        void onEvent(string eventId, string paramKey, string paramValue);

        void logTimeEvent(string eventId, string paramValue);

        void endTimedEvent(string eventId);
        void setAge(int age);

        void setGender(bool isMan);
    }
}