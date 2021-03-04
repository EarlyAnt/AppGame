using System.Collections.Generic;

namespace AppGame.Data.Local
{
    public interface IDeviceInfoManager
    {
        void SaveDeviceInfo(DeviceInfo deviceInfo);
        DeviceInfo GetDeviceInfo(string deviceID);
        List<DeviceInfo> GetAllDeviceInfos();
        int GetDeviceCount();
        int GetMpLimit();
        int GetMpLimit(DeviceTypes deviceType);
        void Clear();
    }

    public enum DeviceTypes
    {
        Walk = 0,
        Ride = 1,
        Train = 2,
        Learn = 3
    }

    public class DeviceInfo
    {
        public string ID { get; set; }
        public DeviceTypes Type { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}