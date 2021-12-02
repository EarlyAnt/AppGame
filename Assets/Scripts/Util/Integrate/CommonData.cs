using System;

public class UnityMessage
{
    public string type { get; set; }
    public string content { get; set; }
}

public class iOSNativeMessage
{
    public string childSN { get; set; }
    public string token { get; set; }
}

public class AndroidNativeMessage
{
    public string childSN { get; set; }
    public string token { get; set; }
}
