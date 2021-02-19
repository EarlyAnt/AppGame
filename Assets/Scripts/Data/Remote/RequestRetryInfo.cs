using System;

namespace AppGame.Data.Remote
{
    public class RequestRetryInfo
    {
        public static int allRetryCount = 3;
        public static int baseRetryIntervalTime = 5000;
    }
}