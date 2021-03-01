using DG.Tweening;
using AppGame.Config;
using AppGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class CityStation : BaseView
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public IMapConfig MapConfig { get; set; }
        [SerializeField]
        private Text fromCityBox;//城市名字文字框
        [SerializeField]
        private Text fromCityPinYinBox;//城市名字文字框
        [SerializeField]
        private Text toCityBox;//景点名字文字框
        [SerializeField]
        private Text toCityPinYinBox;//景点名字文字框
        public System.Action<bool, int, int> ViewClosed { get; set; }//卡片关闭时委托
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //显示卡片
        public void Show(string mapID)
        {
            MapInfo mapInfo = this.MapConfig.GetMap(mapID);
            if (mapInfo == null)
            {
                Debug.LogErrorFormat("<><CityStation.Show>Error: can not find the map[{0}]", mapID);
                return;
            }

            MapInfo nextMapInfo = this.MapConfig.GetMap(mapInfo.NextMap);
            if (mapInfo == null)
            {
                Debug.LogErrorFormat("<><CityStation.Show>Error: can not find the next map[{0}]", mapInfo.NextMap);
                return;
            }

            this.fromCityBox.text = mapInfo.CityName;
            this.toCityBox.text = nextMapInfo.CityName;
            //this.fromCityPinYinBox.text = "";
            //this.toCityPinYinBox.text = "";
            this.gameObject.SetActive(true);
        }
        //旅行
        public void Travel(Vehicle vehicle)
        {
            this.OnViewClosed(true, vehicle.Coin, vehicle.Step);
        }
        //隐藏卡片
        public void Stay()
        {
            this.gameObject.SetActive(false);
            this.OnViewClosed(false, 0, 0);
        }
        //当卡片关闭时
        private void OnViewClosed(bool travel, int coin, int step)
        {
            if (this.ViewClosed != null)
                this.ViewClosed(travel, coin, step);
        }
    }
}
