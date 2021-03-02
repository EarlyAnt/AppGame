using DG.Tweening;
using AppGame.Config;
using AppGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class ScenicCard : BaseView
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口
        [Inject]
        public IMapConfig MapConfig { get; set; }
        [Inject]
        public IScenicConfig ScenicConfig { get; set; }
        [Inject]
        public II18NConfig I18NConfig { get; set; }
        #endregion
        #region 页面UI组件
        [SerializeField]
        private Transform root;//卡片面板根物体
        [SerializeField]
        private GameObject fore;//卡片正面
        [SerializeField]
        private GameObject back;//卡片背面
        [SerializeField, Range(0f, 1f)]
        private float rotateDuration = 1f;//卡片旋转时长
        [SerializeField]
        private Image imageBox;//景点图片框
        [SerializeField]
        private Text cityNameBox;//城市名字文字框
        [SerializeField]
        private Text scenicNameBox;//景点名字文字框
        [SerializeField]
        private Text descriptionBox;//景点介绍文字框
        #endregion
        #region 其他变量
        private Vector3 middleAngle = new Vector3(0f, 90f, 0f);//卡片90度角
        private Vector3 foreAngle = new Vector3(0f, 180f, 0f);//卡片翻转角度
        #endregion
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //显示卡片
        public void Show(string scenicID)
        {
            this.Reset();
            //查询地图和景点数据数据
            ScenicInfo scenicInfo = this.ScenicConfig.GetScenic(scenicID);
            if (scenicInfo == null)
            {

                Debug.LogErrorFormat("<><ScenicCard.Show>Error: can not find scenic named '{0}'", scenicID);
                return;
            }

            MapInfo mapInfo = this.MapConfig.GetMap(scenicInfo.MapID);
            if (mapInfo == null)
            {

                Debug.LogErrorFormat("<><ScenicCard.Show>Error: can not find city named '{0}'", scenicInfo.MapID);
                return;
            }
            //设置页面内容
            this.imageBox.sprite = SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Cycling, string.Format("Texture/Cycling/Site/{0}", scenicInfo.Image)); ;
            this.cityNameBox.text = mapInfo.CityName;
            this.scenicNameBox.text = scenicInfo.Name;
            this.descriptionBox.text = this.I18NConfig.GetText(scenicInfo.Text);
            this.gameObject.SetActive(true);
        }
        //隐藏卡片
        public void Hide()
        {
            this.gameObject.SetActive(false);
            this.Reset();
            this.OnViewClosed();
        }
        //旋转卡片
        public void Rotate()
        {
            this.root.DOScale(1, this.rotateDuration);
            this.root.DOLocalRotate(this.middleAngle, this.rotateDuration / 2).onComplete = () =>
            {
                this.back.SetActive(false);
                this.fore.SetActive(true);
                this.root.DOLocalRotate(this.foreAngle, this.rotateDuration / 2);
            };
        }
        //重设卡片状态
        private void Reset()
        {
            this.root.localEulerAngles = Vector3.zero;
            this.root.localScale = Vector3.one * 0.7f;
            this.fore.SetActive(false);
            this.back.SetActive(true);
        }
        //当卡片关闭时
        private void OnViewClosed()
        {
            this.dispatcher.Dispatch(GameEvent.SCENIC_CARD_CLOSE);
        }
    }
}
