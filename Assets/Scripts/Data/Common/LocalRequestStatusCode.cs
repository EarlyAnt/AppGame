using System;

public class LocalRequestStatusCode
{
    public readonly static int Initial = 11;
    public readonly static int Queued = 12;
    public readonly static int Processing = 13;
    public readonly static int ConnectionTimedOut = 14;
    public readonly static int Error = 15;
    public readonly static int Aborted = 16;
    public readonly static int TimedOut = 17;
}