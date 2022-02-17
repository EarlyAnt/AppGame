using AppGame.Config;
using AppGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class Vehicle : BaseView
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public IModuleConfig ModuleConfig { get; set; }
        [SerializeField]
        private ABSpriteLoader background;
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
        private int coinPrice;//交通费单价：金币
        [SerializeField]
        private int hpPrice;//交通费单价：能量点数
        [SerializeField]
        private string vehicleName;//交通工具名称

        private int coin;//交通费：金币
        private int hp;//交通费：能量点数
        public int Coin
        {
            get { return this.coin; }
            set { this.coin = value; this.coinBox.text = value.ToString(); }
        }
        public int Hp
        {
            get { return this.hp; }
            set { this.hp = value; this.hpBox.text = string.Format("+{0}步", value); }
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
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        public void SetStatus(bool enable)
        {
            string imageName = enable ? this.enableBgName : this.disableBgName;
            this.background.LoadImage(imageName);
            this.button.raycastTarget = enable;
        }
    }
}
