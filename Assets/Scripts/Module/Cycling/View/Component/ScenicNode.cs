using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class ScenicNode : BaseInteraction
    {
        /************************************************�������������************************************************/
        [SerializeField]
        private Image imageBox;
        [SerializeField]
        private Text textBox;
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        public override void Show()
        {
            //Todo: �������ļ��ж�ȡҪ��ʾ��ͼƬ�����֣������ֵĹ��ʻ�����ʾ��Ƭ
        }

        public override void Hide()
        {
            throw new System.NotImplementedException();
        }
    }
}
