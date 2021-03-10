using AppGame.UI;

namespace AppGame.Module.GameStart
{
    public class GameStartMediator : BaseMediator
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public GameStartView View { get; set; }
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        public override void OnRegister()
        {
        }

        public override void OnRemove()
        {
        }
    }
}

