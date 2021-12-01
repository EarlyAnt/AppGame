using AppGame.Data.Model;
using AppGame.Data.Remote;
using strange.extensions.command.impl;
using UnityEngine;

namespace AppGame.Data.Common
{
    public class UploadPlayerDataCommand : Command
    {
        [Inject]
        public ICyclingDataUtil CyclingDataUtil { get; set; }
        [Inject]
        public PlayerData PlayerData { set; get; }

        public override void Execute()
        {
            Retain();

            this.CyclingDataUtil.PutGameData(this.PlayerData, (success) =>
            {
                Debug.Log("<><UploadPlayerDataCommand.Execute>success");
                Release();

            }, (failure) =>
            {
                Debug.LogErrorFormat("<><UploadPlayerDataCommand.Execute>error: {0}, {1}", failure.code, failure.info);
                Release();
            });
        }
    }
}
