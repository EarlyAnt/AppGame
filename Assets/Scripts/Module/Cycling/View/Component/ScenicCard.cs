using AppGame.Config;
using AppGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class ScenicCard : BaseView
    {
        /************************************************�������������************************************************/
        [SerializeField]
        private Image imageBox;
        [SerializeField]
        private Text textBox;
        public System.Action ViewClosed { get; set; }
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        public void Show(Sprite sprite, string text)
        {
            //this.imageBox.sprite = sprite;
            this.textBox.text = text;
            this.gameObject.SetActive(true);
        }
        public void Hide()
        {
            this.gameObject.SetActive(false);
            this.OnViewClosed();
        }

        private void OnViewClosed()
        {
            if (this.ViewClosed != null)
                this.ViewClosed();
        }
    }
}
