using AppGame.UI;

namespace AppGame.Module.GameStart
{
    public class GameStartMediator : BaseMediator
    {
        /************************************************�������������************************************************/
        [Inject]
        public GameStartView View { get; set; }
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        public override void OnRegister()
        {
        }

        public override void OnRemove()
        {
        }
    }
}

