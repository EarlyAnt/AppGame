using DG.Tweening;
using System;
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
        public int Value
        {
            get
            {
                return this.mp;
            }
            set
            {
                this.mp = value;
                this.mpBox.text = value > 999 ? "999" : value.ToString();
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
            if (this.OnCollectMp != null)
            {
                this.OnCollectMp(this);
            }
        }
    }
}
