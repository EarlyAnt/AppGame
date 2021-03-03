using AppGame.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Data.Local
{
    public class LocalDataManager : ILocalDataManager
    {
        private IFileManager fileManager;
        private IPreferencesUtils preferences;
        private JsonUtil jsonUtil;
        private static readonly string DONT_DELETE_LIST_KEY = "DONT_DELETE_LIST_KEY";

        public LocalDataManager()
        {
            this.fileManager = new FileManager();
            this.preferences = new PreferenceUtils();
            this.jsonUtil = new JsonUtil();
        }

        public T GetObject<T>(string key, object defaultObj)
        {
            return this.preferences.GetObject<T>(key, defaultObj);
        }

        public T GetObject<T>(string key)
        {
            return this.preferences.GetObject<T>(key);
        }

        public void SaveObject<T>(string key, T t)
        {
            this.preferences.SaveObject<T>(key, t);
        }

        public void RemoveObject(string key)
        {
            this.preferences.DeleteObject(key);
        }

        public void DeleteAll()
        {
            DeleteAll(false);
        }

        public void DeleteAll(bool isRealDeleteAll)
        {
            if (isRealDeleteAll)
            {
                this.preferences.DeleteAll();
            }
            else
            {
                List<string> dontDeleteKeyList = this.preferences.GetObject<List<string>>(DONT_DELETE_LIST_KEY);
                if (dontDeleteKeyList == null || dontDeleteKeyList.Count == 0)
                {
                    this.preferences.DeleteAll();
                }
                else
                {
                    List<DontDeleteDataItem> saveDontDeleteList = new List<DontDeleteDataItem>();
                    foreach (string key in dontDeleteKeyList)
                    {
                        DontDeleteDataItem dontDeleteDataItem = this.preferences.GetObject<DontDeleteDataItem>(key);
                        saveDontDeleteList.Add(dontDeleteDataItem);
                    }
                    this.preferences.DeleteAll();
                    dontDeleteKeyList.Clear();

                    foreach (DontDeleteDataItem item in saveDontDeleteList)
                    {
                        this.preferences.SaveObject(item.Key, item);
                        dontDeleteKeyList.Add(item.Key);
                    }
                    this.preferences.SaveObject(DONT_DELETE_LIST_KEY, dontDeleteKeyList);
                }
            }
        }
    }
}