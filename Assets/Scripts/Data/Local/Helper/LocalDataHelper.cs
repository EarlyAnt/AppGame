using AppGame.Data.Common;
using System.Collections.Generic;

namespace AppGame.Data.Local
{
    public class LocalDataHelper : ILocalDataHelper
    {
        [Inject]
        public IPreferenceHelper PreferencesUtil { get; set; }
        private IFileHelper fileHelper;
        private static readonly string DONT_DELETE_LIST_KEY = "DONT_DELETE_LIST_KEY";

        public LocalDataHelper()
        {
            this.fileHelper = new FileHelper();
        }

        public T GetObject<T>(string key, object defaultObj)
        {
            return this.PreferencesUtil.GetObject<T>(key, defaultObj);
        }

        public T GetObject<T>(string key)
        {
            return this.PreferencesUtil.GetObject<T>(key);
        }

        public void SaveObject<T>(string key, T t)
        {
            this.PreferencesUtil.SaveObject<T>(key, t);
        }

        public void RemoveObject(string key)
        {
            this.PreferencesUtil.DeleteObject(key);
        }

        public void DeleteAll()
        {
            DeleteAll(false);
        }

        public void DeleteAll(bool isRealDeleteAll)
        {
            if (isRealDeleteAll)
            {
                this.PreferencesUtil.DeleteAll();
            }
            else
            {
                List<string> dontDeleteKeyList = this.PreferencesUtil.GetObject<List<string>>(DONT_DELETE_LIST_KEY);
                if (dontDeleteKeyList == null || dontDeleteKeyList.Count == 0)
                {
                    this.PreferencesUtil.DeleteAll();
                }
                else
                {
                    List<DontDeleteDataItem> saveDontDeleteList = new List<DontDeleteDataItem>();
                    foreach (string key in dontDeleteKeyList)
                    {
                        DontDeleteDataItem dontDeleteDataItem = this.PreferencesUtil.GetObject<DontDeleteDataItem>(key);
                        saveDontDeleteList.Add(dontDeleteDataItem);
                    }
                    this.PreferencesUtil.DeleteAll();
                    dontDeleteKeyList.Clear();

                    foreach (DontDeleteDataItem item in saveDontDeleteList)
                    {
                        this.PreferencesUtil.SaveObject(item.Key, item);
                        dontDeleteKeyList.Add(item.Key);
                    }
                    this.PreferencesUtil.SaveObject(DONT_DELETE_LIST_KEY, dontDeleteKeyList);
                }
            }
        }
    }
}