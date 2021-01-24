using strange.extensions.mediation.impl;

namespace AppGame.Module.Cycling
{
    public class CyclingMediator : EventMediator
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public CyclingView View { get; set; }

        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        public override void OnRegister()
        {
            UpdateListeners(true);
        }

        public override void OnRemove()
        {
            UpdateListeners(false);
        }

        private void UpdateListeners(bool value)
        {
            //view.dispatcher.UpdateListener(value, GameStartView.CLICK_EVENT, onViewClicked);
            //dispatcher.UpdateListener(value, GameEvent.GAME_UPDATE, onGameUpdate);
            //dispatcher.UpdateListener(value, GameEvent.GAME_OVER, onGameOver);

            //dispatcher.AddListener(GameEvent.RESTART_GAME, onRestart);
        }

        private void onViewClicked()
        {
            //dispatcher.Dispatch(GameEvent.SHIP_DESTROYED);
        }

        private void onGameUpdate()
        {
        }

        private void onGameOver()
        {
            UpdateListeners(false);
        }

        private void onRestart()
        {
            OnRegister();
        }
    }
}

