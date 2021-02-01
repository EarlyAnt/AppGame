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
            this.GetGameData();
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

        private void GetGameData()
        {
            LocationDatas locationDatas = new LocationDatas();
            locationDatas.Datas.Add(new LocationData() { UserID = "01", AvatarID = "", MapPointID = "320101_01" });
            locationDatas.Datas.Add(new LocationData() { UserID = "02", AvatarID = "avatar06", MapPointID = "320101_34" });
            locationDatas.Datas.Add(new LocationData() { UserID = "03", AvatarID = "avatar09", MapPointID = "320101_12" });
            this.View.LocationDatas = locationDatas;
        }
    }
}

