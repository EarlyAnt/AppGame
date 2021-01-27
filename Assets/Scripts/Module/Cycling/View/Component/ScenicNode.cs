using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class ScenicNode : BaseInteraction
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private Image imageBox;
        [SerializeField]
        private Text textBox;
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        public override void Show()
        {
            //Todo: 从配置文件中读取要显示的图片，文字，及文字的国际化，显示卡片
        }

        public override void Hide()
        {
            throw new System.NotImplementedException();
        }
    }
}
