using DG.Tweening;
using AppGame.Config;
using AppGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class PayBill : BaseView
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口
        [Inject]
        public IMapConfig MapConfig { get; set; }
        [Inject]
        public IScenicConfig ScenicConfig { get; set; }
        [Inject]
        public ICardConfig CardConfig { get; set; }
        [Inject]
        public II18NConfig I18NConfig { get; set; }
        [Inject]
        public IModuleConfig ModuleConfig { get; set; }
        #endregion
        #region 页面UI组件
        [SerializeField]
        private Transform root;//卡片面板根物体
        [SerializeField]
        private Text titleBox;//标题文字框
        [SerializeField]
        private Text tipBox;//说明文字框
        [SerializeField]
        private Text mpBox;//能量文字框
        [SerializeField]
        private Text feepBox;//费用文字框
        [SerializeField]
        private Image buttonBox;//费用边框
        #endregion
        #region 其他变量
        #endregion
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //显示卡片
        public void Show(int mp, int coin, bool coinEnough)
        {
            //设置页面内容
            this.mpBox.text = mp.ToString();
            this.feepBox.text = coin.ToString();
            //this.titleBox.text = "";//Todo: 后续补上国际化的功能
            //this.tipBox.text = "";//Todo: 后续补上国际化的功能
            Sprite sprite = SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Cycling, this.ModuleConfig.GetImagePath(ModuleViews.Cycling, coinEnough ? "enable_button" : "disable_button"));
            this.buttonBox.sprite = sprite;
            this.buttonBox.raycastTarget = coinEnough;
            this.gameObject.SetActive(true);
        }
        //隐藏卡片
        public void Hide()
        {
            this.gameObject.SetActive(false);
            this.OnViewClosed();
        }
        //当卡片关闭时
        private void OnViewClosed()
        {
            this.dispatcher.Dispatch(GameEvent.PAY_BILL_CLOSE);
        }
    }
}
