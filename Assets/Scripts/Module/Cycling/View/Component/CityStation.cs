using DG.Tweening;
using AppGame.Config;
using AppGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class CityStation : BaseView
    {
        /************************************************�������������************************************************/
        [Inject]
        public IMapConfig MapConfig { get; set; }
        [SerializeField]
        private GameObject root;
        [SerializeField]
        private Text fromCityBox;//�����������ֿ�
        [SerializeField]
        private Text fromCityPinYinBox;//�����������ֿ�
        [SerializeField]
        private Text toCityBox;//�����������ֿ�
        [SerializeField]
        private Text toCityPinYinBox;//�����������ֿ�
        private Ticket ticket;
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        //��ʾ��Ƭ
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
        //����
        public void Travel(Vehicle vehicle)
        {
            this.ticket.Go = true;
            this.ticket.Coin = vehicle.Coin;
            this.ticket.Step = vehicle.Step;
            this.ticket.Vehicle = vehicle.VehicleName;
            this.dispatcher.Dispatch(GameEvent.CITY_STATION_CLOSE, this.ticket);
            this.root.SetActive(false);
        }
        //���ؿ�Ƭ
        public void Stay()
        {
            this.ticket.Go = false;
            this.dispatcher.Dispatch(GameEvent.CITY_STATION_CLOSE, this.ticket);
            this.root.SetActive(false);
        }
    }
}
