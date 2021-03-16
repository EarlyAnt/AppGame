using AppGame.Config;
using AppGame.UI;
using System.Collections.Generic;
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
        [SerializeField]
        private List<Vehicle> vehicles;//交通工具集合
        private int coin;
        private int hp;
        private Ticket ticket;
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //显示卡片
        public void Show(string mapID, int coin, int hp)
        {
            //接收参数
            this.dispatcher.Dispatch(GameEvent.SET_TOUCH_PAD_ENABLE, false);
            this.coin = coin;
            this.hp = hp;
            //检查交通工具是否可用
            this.CheckVehicles();
            //显示出发地和目的地名称及拼音
            MapInfo mapInfo = this.MapConfig.GetMap(mapID);
            if (mapInfo == null)
            {
                Debug.LogErrorFormat("<><CityStation.Show>Error: can not find the map[{0}]", mapID);
                return;
            }

            MapInfo nextMapInfo = this.MapConfig.GetMap(mapInfo.NextMap);
            if (nextMapInfo == null)
            {
                Debug.LogErrorFormat("<><CityStation.Show>Error: can not find the next map[{0}]", mapInfo.NextMap);
                return;
            }

            this.fromCityBox.text = mapInfo.CityName;
            this.fromCityPinYinBox.text = mapInfo.CityPinYin;
            this.toCityBox.text = nextMapInfo.CityName;
            this.toCityPinYinBox.text = nextMapInfo.CityPinYin;
            this.root.SetActive(true);
            //生成车票数据
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
            this.ticket.Hp = vehicle.Hp;
            this.ticket.Vehicle = vehicle.VehicleName;
            this.dispatcher.Dispatch(GameEvent.CITY_STATION_CLOSE, this.ticket);
            this.root.SetActive(false);
            this.dispatcher.Dispatch(GameEvent.SET_TOUCH_PAD_ENABLE, true);
        }
        //隐藏卡片
        public void Stay()
        {
            this.ticket.Go = false;
            this.dispatcher.Dispatch(GameEvent.CITY_STATION_CLOSE, this.ticket);
            this.root.SetActive(false);
            this.dispatcher.Dispatch(GameEvent.SET_TOUCH_PAD_ENABLE, true);
        }
        //检查交通工具是否可用
        private void CheckVehicles()
        {
            foreach (var vehicle in this.vehicles)
            {
                vehicle.SetStatus(this.coin >= vehicle.Coin && this.hp >= vehicle.Hp);
            }
        }
    }
}
