using AppGame.Config;
using AppGame.UI;
using System.Collections.Generic;
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
        [SerializeField]
        private List<Vehicle> vehicles;//��ͨ���߼���
        private int coin;
        private int hp;
        private Ticket ticket;
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        //��ʾ��Ƭ
        public void Show(string mapID, int coin, int hp)
        {
            //���ղ���
            this.dispatcher.Dispatch(GameEvent.SET_TOUCH_PAD_ENABLE, false);
            this.coin = coin;
            this.hp = hp;
            //��齻ͨ�����Ƿ����
            this.CheckVehicles();
            //��ʾ�����غ�Ŀ�ĵ����Ƽ�ƴ��
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
            //���ɳ�Ʊ����
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
            this.ticket.Hp = vehicle.Hp;
            this.ticket.Vehicle = vehicle.VehicleName;
            this.dispatcher.Dispatch(GameEvent.CITY_STATION_CLOSE, this.ticket);
            this.root.SetActive(false);
            this.dispatcher.Dispatch(GameEvent.SET_TOUCH_PAD_ENABLE, true);
        }
        //���ؿ�Ƭ
        public void Stay()
        {
            this.ticket.Go = false;
            this.dispatcher.Dispatch(GameEvent.CITY_STATION_CLOSE, this.ticket);
            this.root.SetActive(false);
            this.dispatcher.Dispatch(GameEvent.SET_TOUCH_PAD_ENABLE, true);
        }
        //��齻ͨ�����Ƿ����
        private void CheckVehicles()
        {
            foreach (var vehicle in this.vehicles)
            {
                vehicle.SetStatus(this.coin >= vehicle.Coin && this.hp >= vehicle.Hp);
            }
        }
    }
}
