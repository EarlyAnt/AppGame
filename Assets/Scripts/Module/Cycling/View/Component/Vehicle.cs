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
        private int coin;//��ͨ�ѣ����
        [SerializeField]
        private int hp;//��ͨ�ѣ���������
        [SerializeField]
        private string vehicleName;//��ͨ��������

        public int Coin { get { return this.coin; } }
        public int Hp { get { return this.hp; } }
        public string VehicleName { get { return this.vehicleName; } }
        /************************************************Unity�������¼�***********************************************/
        protected override void Start()
        {
            this.coinBox.text = this.coin.ToString();
            this.hpBox.text = this.hp.ToString();
        }
        /************************************************�� �� �� �� ��************************************************/
        public void SetStatus(bool enable)
        {
            string imageName = enable ? this.enableBgName : this.disableBgName;
            this.background.sprite = SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Cycling, this.ModuleConfig.GetImagePath(ModuleViews.Cycling, imageName));
            this.button.raycastTarget = enable;
        }
    }
}
