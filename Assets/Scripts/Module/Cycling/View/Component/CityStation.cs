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
        private GameObject root;
        [SerializeField]
        private Text fromCityBox;//城市名字文字框
        [SerializeField]
        private Text fromCityPinYinBox;//城市名字文字框
        [SerializeField]
        private Text toCityBox;//景点名字文字框
        [SerializeField]
        private Text toCityPinYinBox;//景点名字文字框
        private Ticket ticket;
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
            this.root.SetActive(true);

            this.ticket = new Ticket()
            {
                FromMapID = mapInfo.ID,
                FromCityName = mapInfo.CityName,
                FromCityPinYin = this.fromCityPinYinBox.text,
                ToMapID = nextMapInfo.ID,
                ToCityName = nextMapInfo.CityName,
                ToCityPinYin = this.toCityPinYinBox.text,
            };
        }
        //旅行
        public void Travel(Vehicle vehicle)
        {
            this.ticket.Go = true;
            this.ticket.Coin = vehicle.Coin;
            this.ticket.Step = vehicle.Step;
            this.ticket.Vehicle = vehicle.VehicleName;
            this.dispatcher.Dispatch(GameEvent.CITY_STATION_CLOSE, this.ticket);
            this.root.SetActive(false);
        }
        //隐藏卡片
        public void Stay()
        {
            this.ticket.Go = false;
            this.dispatcher.Dispatch(GameEvent.CITY_STATION_CLOSE, this.ticket);
            this.root.SetActive(false);
        }
    }
}
