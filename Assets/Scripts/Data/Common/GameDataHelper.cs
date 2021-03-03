using AppGame.Data.Local;
using AppGame.Util;
using UnityEngine;

namespace AppGame.Data.Common
{
    public class GameDataHelper : IGameDataHelper
    {
        [Inject]
        public IChildInfoManager ChildInfoManager { get; set; }
        [Inject]
        public ILocalDataHelper LocalDataHelper { get; set; }
        [Inject]
        public IJsonUtil JsonUtil { get; set; }

        public T GetObject<T>(string key)
        {
            T t = LocalDataHelper.GetObject<T>(this.ChildInfoManager.GetChildSN() + key);
            //Debug.LogFormat("<><GameDataHelper.GetObject>Key: {0}, Value: {1}", this.ChildInfoManager.GetChildSN() + key, t.ToString());
            return t;
        }
        public T GetObject<T>(string key, object defaultObj)
        {
            T t = LocalDataHelper.GetObject<T>(this.ChildInfoManager.GetChildSN() + key, defaultObj);
            //Debug.LogFormat("<><GameDataHelper.GetObject>Key: {0}, Value: {1}", this.ChildInfoManager.GetChildSN() + key, t.ToString());
            return t;
        }
        public void SaveObject<T>(string key, T t)
        {
            LocalDataHelper.SaveObject<T>(this.ChildInfoManager.GetChildSN() + key, t);
            //Debug.LogFormat("<><GameDataHelper.SaveObject>Key: {0}, Value: {1}", this.ChildInfoManager.GetChildSN() + key, t.ToString());
        }
        public void Clear(string key)
        {
            LocalDataHelper.RemoveObject(this.ChildInfoManager.GetChildSN() + key);
        }
    }
}
