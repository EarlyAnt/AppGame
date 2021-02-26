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
        private Text textBox;//景点介绍文字框
        private Vector3 middleAngle = new Vector3(0f, 90f, 0f);//卡片90度角
        private Vector3 foreAngle = new Vector3(0f, 180f, 0f);//卡片翻转角度
        public System.Action ViewClosed { get; set; }//卡片关闭时委托
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //显示卡片
        public void Show(Sprite sprite, string text)
        {
            this.Reset();
            //this.imageBox.sprite = sprite;
            this.textBox.text = text;
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
            if (this.ViewClosed != null)
                this.ViewClosed();
        }
    }
}
