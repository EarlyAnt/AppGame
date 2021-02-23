using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class MpBall : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private MpBallTypes mpBallType;
        [SerializeField]
        private Text mpBox;
        [SerializeField]
        private Text fromBox;
        private int mp = 0;
        private string fromName = "";
        public int Value
        {
            get { return this.mp; }
            set { this.mp = value; this.mpBox.text = value.ToString(); }
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
        /************************************************Unity方法与事件***********************************************/
        private void Awake()
        {
            this.mpBox.text = "0";
        }
        /************************************************自 定 义 方 法************************************************/

    }
}
