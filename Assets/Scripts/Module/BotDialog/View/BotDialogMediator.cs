using AppGame.Config;
using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Data.Remote;
using AppGame.UI;
using strange.extensions.dispatcher.eventdispatcher.api;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Module.BotDialog
{
    public class BotDialogMediator : BaseMediator
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口
        [Inject]
        public BotDialogView View { get; set; }
        #endregion
        #region 其他变量

        #endregion
        /************************************************Unity方法与事件***********************************************/
        
        /************************************************自 定 义 方 法************************************************/
        //注册
        public override void OnRegister()
        {
            UpdateListeners(true);
        }
        //取消注册
        public override void OnRemove()
        {
            UpdateListeners(false);
        }
        //注册事件监听
        private void UpdateListeners(bool register)
        {
            //this.dispatcher.UpdateListener(register, GameEvent.GAME_START, this.RestartGame);
            //this.dispatcher.UpdateListener(register, GameEvent.MP_CLICK, this.OnMpBallClick);
            //this.dispatcher.UpdateListener(register, GameEvent.GO_CLICK, this.OnGo);
            //this.dispatcher.UpdateListener(register, GameEvent.GO_BUTTON_LOADED, this.RefreshMpAndHp);
            //this.dispatcher.UpdateListener(register, GameEvent.COLLECT_MP, this.OnCollectMp);
            //this.dispatcher.UpdateListener(register, GameEvent.INTERACTION, this.OnPlayerStopped);
            //this.dispatcher.UpdateListener(register, GameEvent.SCENIC_CARD_CLOSE, this.OnScenicCardClosed);
            //this.dispatcher.UpdateListener(register, GameEvent.CITY_STATION_CLOSE, this.OnCityStationClosed);
        }        
    }
}
