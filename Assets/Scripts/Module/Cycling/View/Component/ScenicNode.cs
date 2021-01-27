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
        /************************************************Unity�������¼�***********************************************/
        protected override void Start()
        {
            base.Start();
        }
        protected override void OnDestroy()
        {
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
    }
}
