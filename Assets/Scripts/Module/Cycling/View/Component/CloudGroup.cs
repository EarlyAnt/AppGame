using DG.Tweening;
using UnityEngine;

namespace AppGame.Module.Cycling
{
    public class CloudGroup : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        #region 页面UI组件
        [SerializeField]
        private CanvasGroup canvasGroup;
        #endregion
        #region 其他变量
        private Tweener fadeTweener;
        public bool Visible { get; private set; }
        #endregion
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //设置是否可见
        public void SetStatus(bool visible)
        {
            this.Visible = visible;

            if (this.fadeTweener != null) this.fadeTweener.Kill();
            this.fadeTweener = this.canvasGroup.DOFade(visible ? 1f : 0f, visible ? 0f : 1f);
        }
        [ContextMenu("添加CanvasGroup")]
        private void AddCanvasGroup()
        {
            if (this.canvasGroup == null)
            {
                this.canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
            }
        }
    }
}
