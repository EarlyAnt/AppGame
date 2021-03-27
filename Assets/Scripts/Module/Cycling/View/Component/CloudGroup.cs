using AppGame.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class CloudGroup : BaseView
    {
        /************************************************属性与变量命名************************************************/
        #region 页面UI组件
        [SerializeField]
        private List<Cloud> clouds;
        [SerializeField, Range(1f, 10f)]
        private float distance = 2f;
        [SerializeField, Range(0f, 10f)]
        private float duration = 0.5f;
        #endregion
        #region 其他变量
        private List<Tweener> tweeners;
        private int cloudCount
        {
            get { return this.clouds != null ? this.clouds.Count : 0; }
        }
        public bool Visible { get; private set; }
        #endregion
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //设置是否可见
        public void SetStatus(bool visible)
        {
            this.Visible = visible;

            if (this.tweeners == null)
            {
                this.tweeners = new List<Tweener>();
            }
            else
            {
                this.tweeners.ForEach(t => t.Kill());
                this.tweeners.Clear();
            }

            this.StopCoroutine("CloudAnimation");
            this.StartCoroutine(CloudAnimation(visible));
        }
        //云朵散开动画
        private IEnumerator CloudAnimation(bool visible)
        {
            int completeCount = 0;
            for (int i = 0; i < this.clouds.Count; i++)
            {
                float durationOffset = Random.Range(1f, 3f);
                Tweener fadeTweener = this.clouds[i].Image.DOFade(visible ? this.clouds[i].OriginAlpha : 0f, visible ? 0f : this.duration * durationOffset);
                Tweener scaleTweener = this.clouds[i].Image.transform.DOScale(visible ? this.clouds[i].OriginScale : this.clouds[i].OriginScale * 0.25f, visible ? 0f : this.duration * durationOffset * 2f);
                Tweener disperseTweener = this.clouds[i].Image.transform.DOLocalMove(visible ? this.clouds[i].OriginPoistion :
                                                                                               this.clouds[i].OriginPoistion * this.distance,
                                                                                     visible ? 0f : this.duration * durationOffset);
                disperseTweener.onComplete = () => { completeCount++; Debug.LogFormat("* * * {0}", completeCount); };
                this.tweeners.Add(fadeTweener);
                this.tweeners.Add(scaleTweener);
                this.tweeners.Add(disperseTweener);
            }
            Debug.LogFormat("+ + + + +");
            yield return new WaitUntil(() => completeCount >= this.cloudCount);
            Debug.LogFormat("- - - - -");
            if (!visible) this.dispatcher.Dispatch(GameEvent.CLOUD_DISPERSE);
        }
        [ContextMenu("收集云朵")]
        private void CollectCloud()
        {
            if (this.clouds == null)
                this.clouds = new List<Cloud>();
            this.clouds.Clear();

            Image[] cloudImages = this.GetComponentsInChildren<Image>();
            if (cloudImages != null && cloudImages.Length > 0)
            {
                foreach (Image cloud in cloudImages)
                {
                    this.clouds.Add(new Cloud()
                    {
                        Image = cloud,
                        OriginPoistion = cloud.transform.localPosition,
                        OriginScale = cloud.transform.localScale,
                        OriginAlpha = cloud.color.a
                    });
                }
            }
            else
            {
                Debug.LogError("<><CloudGroup.CollectCloud>Error: can not find Image component in children");
            }
        }
    }

    [System.Serializable]
    public class Cloud
    {
        public Image Image;
        public Vector3 OriginPoistion;
        public Vector3 OriginScale;
        public float OriginAlpha;
    }
}
