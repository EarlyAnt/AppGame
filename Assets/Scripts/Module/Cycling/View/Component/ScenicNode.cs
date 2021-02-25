using AppGame.Config;
using UnityEngine;

namespace AppGame.Module.Cycling
{
    public class ScenicNode : BaseInteraction
    {
        /************************************************�������������************************************************/
        [Inject]
        public IScenicConfig ScenicConfig { get; set; }
        [Inject]
        public II18NConfig I18NConfig { get; set; }
        [SerializeField]
        private ScenicCard scenicCard;
        public System.Action CardViewClosed { get; set; }
        /************************************************Unity�������¼�***********************************************/
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
        /************************************************�� �� �� �� ��************************************************/
        public override void Show()
        {
            ScenicInfo scenicInfo = this.ScenicConfig.GetScenic(this.ID);
            if (scenicInfo == null)
            {
                Debug.LogErrorFormat("<><ScenicNode.Show>Error: can not find scenic named '{0}'", this.ID);
                return;
            }
            this.scenicCard.Show(null, this.I18NConfig.GetText(scenicInfo.Text));
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
