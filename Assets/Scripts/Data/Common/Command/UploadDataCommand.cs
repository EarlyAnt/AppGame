using AppGame.Data.Model;
using AppGame.Data.Remote;
using strange.extensions.command.impl;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Data.Common
{
    public class UploadItemDataCommand : Command
    {
        [Inject]
        public IItemDataUtil ItemDataUtil { get; set; }
        [Inject]
        public List<ItemData> ItemDataList { set; get; }

        public override void Execute()
        {
            Retain();

            this.ItemDataUtil.PutItemData(this.ItemDataList, (success) =>
            {
                Debug.Log("<><UploadItemDataCommand.Execute>success");
                Release();

            }, (failure) =>
            {
                Debug.LogErrorFormat("<><UploadItemDataCommand.Execute>error: {0}, {1}", failure.code, failure.info);
                Release();
            });
        }
    }
}
