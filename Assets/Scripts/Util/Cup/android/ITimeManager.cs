namespace Cup.Utils.android
{
    public interface ITimeManager
    {
         void setTimeZone(string timeZone);

         void setServerTime(long millisTime);
    }
}