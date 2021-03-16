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
        private int coin;//交通费：金币
        [SerializeField]
        private int hp;//交通费：能量点数
        [SerializeField]
        private string vehicleName;//交通工具名称

        public int Coin { get { return this.coin; } }
        public int Hp { get { return this.hp; } }
        public string VehicleName { get { return this.vehicleName; } }
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
            this.coinBox.text = this.coin.ToString();
            this.hpBox.text = this.hp.ToString();
        }
        /************************************************自 定 义 方 法************************************************/
        public void SetStatus(bool enable)
        {
            string imageName = enable ? this.enableBgName : this.disableBgName;
            this.background.sprite = SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Cycling, this.ModuleConfig.GetImagePath(ModuleViews.Cycling, imageName));
            this.button.raycastTarget = enable;
        }
    }
}
