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
        /************************************************�������������************************************************/
        #region ע��ӿ�
        [Inject]
        public BotDialogView View { get; set; }
        #endregion
        #region ��������

        #endregion
        /************************************************Unity�������¼�***********************************************/
        
        /************************************************�� �� �� �� ��************************************************/
        //ע��
        public override void OnRegister()
        {
            UpdateListeners(true);
        }
        //ȡ��ע��
        public override void OnRemove()
        {
            UpdateListeners(false);
        }
        //ע���¼�����
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
