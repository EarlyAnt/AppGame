using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class MpBall : MonoBehaviour
    {
        /************************************************�������������************************************************/
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
            get { return this.mpBallType; }
            set { this.mpBallType = value; }
        }
        public string FromID { get; set; }
        public string FromName
        {
            get { return this.fromName; }
            set { this.fromName = value; this.fromBox.text = value; }
        }
        /************************************************Unity�������¼�***********************************************/
        private void Awake()
        {
            this.mpBox.text = "0";
        }
        /************************************************�� �� �� �� ��************************************************/

    }
}
