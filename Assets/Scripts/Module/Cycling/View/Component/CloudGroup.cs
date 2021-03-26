using DG.Tweening;
using UnityEngine;

namespace AppGame.Module.Cycling
{
    public class CloudGroup : MonoBehaviour
    {
        /************************************************�������������************************************************/
        #region ҳ��UI���
        [SerializeField]
        private CanvasGroup canvasGroup;
        #endregion
        #region ��������
        private Tweener fadeTweener;
        public bool Visible { get; private set; }
        #endregion
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        //�����Ƿ�ɼ�
        public void SetStatus(bool visible)
        {
            this.Visible = visible;

            if (this.fadeTweener != null) this.fadeTweener.Kill();
            this.fadeTweener = this.canvasGroup.DOFade(visible ? 1f : 0f, visible ? 0f : 1f);
        }
        [ContextMenu("���CanvasGroup")]
        private void AddCanvasGroup()
        {
            if (this.canvasGroup == null)
            {
                this.canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
            }
        }
    }
}
