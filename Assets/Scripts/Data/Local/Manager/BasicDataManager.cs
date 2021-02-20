using AppGame.Data.Model;
using System;
using System.Collections.Generic;

namespace AppGame.Data.Local
{
    public class BasicDataManager : IBasicDataManager
    {
        private List<BasicData> basicDataList = null;

        /// <summary>
        /// 保存一组玩家的基础数据
        /// </summary>
        /// <param name="basicDataList"></param>
        public void SaveDataList(List<BasicData> basicDataList)
        {
            if (basicDataList != null)
                this.basicDataList = basicDataList;
            else
                throw new ArgumentException("<><BasicDataManager.SaveDataList>Error: parameter 'basicDataList' is null");
        }
        /// <summary>
        /// 获取所有玩家的基础数据
        /// </summary>
        /// <returns></returns>
        public List<BasicData> GetAllData()
        {
            return this.basicDataList;
        }

        /// <summary>
        /// 保存玩家的基础数据
        /// </summary>
        /// <param name="basicData"></param>
        public void SaveData(BasicData basicData)
        {
            if (this.basicDataList == null)
                this.basicDataList = new List<BasicData>();

            int index = this.basicDataList.FindIndex(t => t.child_sn == basicData.child_sn);
            if (index >= 0 && index < this.basicDataList.Count)
                this.basicDataList.RemoveAt(index);

            this.basicDataList.Add(basicData);
        }
        /// <summary>
        /// 根据编号获取玩家的基础数据
        /// </summary>
        /// <param name="childSN">玩家编号</param>
        /// <returns></returns>
        public BasicData GetData(string childSN)
        {
            if (this.basicDataList != null)
                return this.basicDataList.Find(t => t.child_sn == childSN);
            else
                return null;
        }
    }
}
