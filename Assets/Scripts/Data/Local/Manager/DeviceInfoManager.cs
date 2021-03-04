using AppGame.Data.Common;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AppGame.Data.Local
{
    public class DeviceInfoManager : IDeviceInfoManager
    {
        [Inject]
        public IGameDataHelper GameDataHelper { get; set; }
        private const string DATA_KEY = "device_info";

        [PostConstruct]
        public void Initialize()
        {
            List<DeviceInfo> deviceInfos = this.GetAllDeviceInfos();
            if (deviceInfos == null) deviceInfos = new List<DeviceInfo>();
            deviceInfos.Add(new DeviceInfo() { ID = "01", Type = DeviceTypes.Walk, Name = "±³±³¼Ñ", Count = 1 });
            deviceInfos.Add(new DeviceInfo() { ID = "02", Type = DeviceTypes.Ride, Name = "Æ½ºâ³µ", Count = 1 });
            deviceInfos.ForEach(t => this.SaveDeviceInfo(t));
        }

        public void SaveDeviceInfo(DeviceInfo deviceInfo)
        {
            if (deviceInfo == null)
            {
                Debug.LogError("<><DeviceInfoManager.SaveDeviceInfo>Error: parameter 'deviceInfo' is null");
                return;
            }

            List<DeviceInfo> deviceInfos = this.GetAllDeviceInfos();
            if (deviceInfos == null)
                deviceInfos = new List<DeviceInfo>();

            DeviceInfo device = deviceInfos.Find(t => t.ID == deviceInfo.ID);
            if (device != null)
                deviceInfos.Remove(device);

            deviceInfos.Add(deviceInfo);
            this.GameDataHelper.SaveObject<List<DeviceInfo>>(DATA_KEY, deviceInfos);
        }

        public DeviceInfo GetDeviceInfo(string deviceID)
        {
            List<DeviceInfo> deviceInfos = this.GetAllDeviceInfos();
            if (deviceInfos != null)
            {
                DeviceInfo deviceInfo = deviceInfos.Find(t => t.ID == deviceID);
                return deviceInfo;
            }
            else return null;
        }

        public List<DeviceInfo> GetAllDeviceInfos()
        {
            List<DeviceInfo> deviceInfos = this.GameDataHelper.GetObject<List<DeviceInfo>>(DATA_KEY);
            return deviceInfos;
        }

        public int GetDeviceCount()
        {
            List<DeviceInfo> deviceInfos = this.GetAllDeviceInfos();
            if (deviceInfos != null)
            {
                int count = deviceInfos.Sum(t => t.Count);
                return count;
            }
            else return 0;
        }

        public int GetMpLimit()
        {
            List<DeviceInfo> deviceInfos = this.GetAllDeviceInfos();
            if (deviceInfos != null)
            {
                int mpLimit = 0;
                deviceInfos.ForEach(t => mpLimit += this.GetMpLimit(t.Type));
                return mpLimit;
            }
            else return 0;
        }

        public void Clear()
        {
            this.GameDataHelper.Clear(DATA_KEY);
        }

        public int GetMpLimit(DeviceTypes deviceType)
        {
            int mpLimit = 0;
            switch (deviceType)
            {
                case DeviceTypes.Walk:
                    mpLimit = 300;
                    break;
                case DeviceTypes.Ride:
                    mpLimit = 300;
                    break;
            }
            return mpLimit;
        }
    }
}