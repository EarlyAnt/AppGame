using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class MpBall : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        #region 页面UI组件
        [SerializeField]
        private MpBallTypes mpBallType;
        [SerializeField]
        private Text mpBox;
        [SerializeField]
        private Text reduceBox;
        [SerializeField]
        private Transform reduceDestination;
        [SerializeField]
        private Text fromBox;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private Transform bubbleTransform;
        [SerializeField, Range(0f, 100f)]
        private float destinationY = 2f;
        [SerializeField, Range(0.1f, 5f)]
        private float durationMin = 2f;
        [SerializeField, Range(0.1f, 5f)]
        private float durationMax = 2f;
        #endregion
        #region 其他变量
        private Tweener fadeTweener;
        private Tweener pingPongTweener;
        private float originY;
        private int mp = 0;
        private string fromName = "";
        private bool clicked = false;
        public int Value
        {
            get
            {
                return this.mp;
            }
            set
            {
                this.StopCoroutine("ChangeValue");
                this.StartCoroutine(this.ChangeValue(value));
            }
        }
        public MpBallTypes MpBallType
        {
            get
            {
                return this.mpBallType;
            }
            set
            {
                this.mpBallType = value;
                switch (value)
                {
                    case MpBallTypes.Walk:
                        this.fromBox.text = "步行";
                        break;
                    case MpBallTypes.Ride:
                        this.fromBox.text = "骑行";
                        break;
                    case MpBallTypes.Train:
                        this.fromBox.text = "坐姿训练";
                        break;
                    case MpBallTypes.Learn:
                        this.fromBox.text = "坐姿监测";
                        break;
                }
            }
        }
        public string FromID { get; set; }
        public string FromName
        {
            get { return this.fromName; }
            set { this.fromName = value; this.fromBox.text = value; }
        }
        public bool Visible { get; private set; }
        public CanvasGroup CanvasGroup { get { return this.canvasGroup; } }
        public Action<MpBall> OnCollectMp { get; set; }
        #endregion
        /************************************************Unity方法与事件***********************************************/
        private void Awake()
        {
            this.mpBox.text = "0";
        }
        private void Start()
        {
            this.originY = this.bubbleTransform.localPosition.y;
            this.destinationY += this.originY;
            this.PlayPingPong();
        }
        private void OnDestroy()
        {
            if (this.fadeTweener != null)
                this.fadeTweener.Kill();

            if (this.pingPongTweener != null)
                this.pingPongTweener.Kill();
        }
        /************************************************自 定 义 方 法************************************************/
        //设置是否可见
        public void SetStatus(bool visible)
        {
            this.Visible = visible;
            if (visible) this.clicked = false;//重新显示时，重置clicked状态

            if (this.fadeTweener != null) this.fadeTweener.Kill();
            this.fadeTweener = this.canvasGroup.DOFade(visible ? 1f : 0f, visible ? 0.375f : 0f);
        }
        //开始播放
        public void PlayPingPong()
        {
            this.StopPingPong();
            if (UnityEngine.Random.Range(0, 10) % 2 == 0)
                this.PingPong(this.originY, this.destinationY);
            else
                this.PingPong(this.originY, this.destinationY);
        }
        //停止播放
        public void StopPingPong()
        {
            if (this.pingPongTweener != null)
                this.pingPongTweener.Kill();
        }
        //往返运动
        private void PingPong(float from, float to)
        {
            this.pingPongTweener = this.bubbleTransform.DOLocalMoveY(to, UnityEngine.Random.Range(this.durationMin, this.durationMax));
            this.pingPongTweener.onComplete += () => this.PingPong(to, from);
        }
        //收取能量
        public void CollectMp()
        {
            this.clicked = true;
            this.AutoCollectMp();
        }
        //自动收取能量(点Go按钮时自动收取)
        public void AutoCollectMp()
        {
            if (this.OnCollectMp != null)
            {
                this.OnCollectMp(this);
            }
        }
        //转换成MpData
        public MpData ToMpData()
        {
            return new MpData()
            {
                MpBallType = this.MpBallType,
                FromID = this.FromID,
                FromName = this.FromName,
                Mp = this.Value,
                RefreshView = this.clicked
            };
        }
        //播放数值变化动画
        private IEnumerator ChangeValue(int value)
        {
            if (this.mp > value)
            {
                int currentMp = this.mp;
                this.mp = value;
                this.reduceBox.transform.position = this.mpBox.transform.position;
                this.reduceBox.text = (value - currentMp).ToString();
                this.reduceBox.DOFade(1f, 0f);
                this.reduceBox.DOFade(0f, 1.5f);
                this.reduceBox.transform.DOMoveY(this.reduceDestination.position.y, 1.5f);
                yield return null;
                this.mpBox.text = value > 999 ? "999" : value.ToString();
                this.reduceBox.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0f, 0f);
            }
            else
            {
                this.mp = value;
                this.mpBox.text = value > 999 ? "999" : value.ToString();
            }
            yield return null;
        }
    }
}
