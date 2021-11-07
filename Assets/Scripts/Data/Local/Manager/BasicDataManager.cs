using AppGame.Data.Common;
using AppGame.Data.Model;
using System;
using System.Collections.Generic;

namespace AppGame.Data.Local
{
    public class BasicDataManager : IBasicDataManager
    {
        [Inject]
        public IGameDataHelper GameDataHelper { get; set; }
        private const string BASIC_DATA_DATA_KEY = "basic_data";

        /// <summary>
        /// 保存一组玩家的基础数据
        /// </summary>
        /// <param name="basicDataList"></param>
        public void SaveDataList(List<BasicData> basicDataList)
        {
            if (basicDataList != null)
                this.GameDataHelper.SaveObject<List<BasicData>>(BASIC_DATA_DATA_KEY, basicDataList);
            else
                throw new ArgumentException("<><BasicDataManager.SaveDataList>Error: parameter 'basicDataList' is null");
        }
        /// <summary>
        /// 获取所有玩家的基础数据
        /// </summary>
        /// <returns></returns>
        public List<BasicData> GetAllData()
        {
            return this.GameDataHelper.GetObject<List<BasicData>>(BASIC_DATA_DATA_KEY);
        }

        /// <summary>
        /// 保存玩家的基础数据
        /// </summary>
        /// <param name="basicData"></param>
        public void SaveData(BasicData basicData)
        {
            List<BasicData> basicDataList = this.GameDataHelper.GetObject<List<BasicData>>(BASIC_DATA_DATA_KEY);
            if (basicDataList == null)
                basicDataList = new List<BasicData>();

            int index = basicDataList.FindIndex(t => t.child_sn == basicData.child_sn);
            if (index >= 0 && index < basicDataList.Count)
                basicDataList.RemoveAt(index);

            basicDataList.Add(basicData);
            this.GameDataHelper.SaveObject<List<BasicData>>(BASIC_DATA_DATA_KEY, basicDataList);
        }
        /// <summary>
        /// 根据编号获取玩家的基础数据
        /// </summary>
        /// <param name="childSN">玩家编号</param>
        /// <returns></returns>
        public BasicData GetData(string childSN)
        {
            List<BasicData> basicDataList = this.GameDataHelper.GetObject<List<BasicData>>(BASIC_DATA_DATA_KEY);
            if (basicDataList != null)
                return basicDataList.Find(t => t.child_sn == childSN);
            else
                return null;
        }
    }
}
