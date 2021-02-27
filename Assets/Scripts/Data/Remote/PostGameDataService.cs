using AppGame.Data.Common;
using UnityEngine;
using AppGame.Global;
using System;

namespace AppGame.Data.Remote
{
    public class PostGameDataService : BaseService, IPostGameDataService
    {
        //[Inject]
        //public ILocalChildInfoAgent LocalChildInfoAgent { get; set; }

        public void PostGameData(string jsonString, Action<string> success, Action<ResponseErroInfo> failure)
        {
            string prod_name = AppData.GameName;
            string child_sn = "";//LocalChildInfoAgent.getChildSN();
            string x_cup_sn = "";//CupBuild.getCupSn();

            if (child_sn == string.Empty)
            {
                return;
            }


            Debug.LogFormat("<><PostGameDataService.PostGameData>Data: {0}", jsonString);
            this.NativeOkHttpMethodWrapper.post(UrlProvider.GetGameDataUrl(child_sn), "", jsonString, (info) =>
            {
                if (success != null)
                    success(info);

            }, (error) =>
            {
                if (failure != null)
                    failure(error);
            });

        }
    }
}