using AppGame.Config;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class ScenicNode : BaseInteraction
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public IScenicConfig ScenicConfig { get; set; }
        [Inject]
        public II18NConfig I18NConfig { get; set; }
        [SerializeField]
        private ScenicCard scenicCard;
        public System.Action CardViewClosed { get; set; }
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
            base.Start();
            this.scenicCard.ViewClosed += this.OnCardViewClosed;
        }
        protected override void OnDestroy()
        {
            this.scenicCard.ViewClosed -= this.OnCardViewClosed;
            base.OnDestroy();
        }
        /************************************************自 定 义 方 法************************************************/
        public override void Show()
        {
            ScenicInfo scenicInfo = this.ScenicConfig.GetScenic(this.ID);
            if (scenicInfo == null)
            {
                Debug.LogErrorFormat("<><ScenicNode.Show>Error: can not find scenic named '{0}'", this.ID);
                return;
            }

            Sprite image = SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Cycling, string.Format("Texture/Cycling/Site/{0}", scenicInfo.Image));
            this.scenicCard.Show(image, scenicInfo.Name, this.I18NConfig.GetText(scenicInfo.Text));
        }

        public override void Hide()
        {
            this.scenicCard.Hide();
        }

        private void OnCardViewClosed()
        {
            if (this.CardViewClosed != null)
                this.CardViewClosed();
        }
    }
}
