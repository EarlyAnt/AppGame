using AppGame.Config;
using AppGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class Vehicle : BaseView
    {
        /************************************************�������������************************************************/
        [Inject]
        public IModuleConfig ModuleConfig { get; set; }
        [SerializeField]
        private Image background;
        [SerializeField]
        private Image button;
        [SerializeField]
        private string enableBgName;
        [SerializeField]
        private string disableBgName;
        [SerializeField]
        private Text coinBox;
        [SerializeField]
        private Text hpBox;
        [SerializeField]
        private int coinPrice;//��ͨ�ѵ��ۣ����
        [SerializeField]
        private int hpPrice;//��ͨ�ѵ��ۣ���������
        [SerializeField]
        private string vehicleName;//��ͨ��������

        private int coin;//��ͨ�ѣ����
        private int hp;//��ͨ�ѣ���������
        public int Coin
        {
            get { return this.coin; }
            set { this.coin = value; this.coinBox.text = value.ToString(); }
        }
        public int Hp
        {
            get { return this.hp; }
            set { this.hp = value; this.hpBox.text = string.Format("+{0}��", value); }
        }
        public int CoinPrice
        {
            get { return this.coinPrice; }
        }
        public int HpPrice
        {
            get { return this.hpPrice; }
        }
        public string VehicleName
        {
            get { return this.vehicleName; }
        }
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        public void SetStatus(bool enable)
        {
            string imageName = enable ? this.enableBgName : this.disableBgName;
            this.background.sprite = SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Cycling, this.ModuleConfig.GetImagePath(ModuleViews.Cycling, imageName));
            this.button.raycastTarget = enable;
        }
    }
}
