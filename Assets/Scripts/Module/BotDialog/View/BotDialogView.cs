using AppGame.Data.Remote;
using AppGame.UI;
using AppGame.Util;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.BotDialog
{
    public class BotDialogView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口
        [Inject]
        public IJsonUtil JsonUtil { get; set; }
        [Inject]
        public IBotDialogDataUtil BotDialogDataUtil { get; set; }
        #endregion
        #region 页面UI组件
        [SerializeField]
        private InputField questionBox;
        [SerializeField]
        private InputField answerBox;
        #endregion
        #region 其他变量
        private string sessionId = "";
        #endregion
        /************************************************Unity方法与事件***********************************************/
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        /************************************************自 定 义 方 法************************************************/
        public void SendDialog()
        {
            DialogRequest dialogRequest = new DialogRequest()
            {
                sessionId = this.sessionId,
                actionAreaId = "default",
                channelId = "default",
                input = new DialogInput() { query = new QueryRequest { queryText = this.questionBox.text } }
            };

            this.BotDialogDataUtil.PostDialogData(dialogRequest,
                (dialogResponse) =>
                {
                    this.sessionId = dialogResponse.sessionId;
                    StringBuilder strbAnswers = new StringBuilder();
                    foreach (ResponseText responseText in dialogResponse.queryResult.responseText)
                    {
                        ResponseText.Content content = this.JsonUtil.String2Json<ResponseText.Content>(responseText.content);
                        if (content != null)
                        {
                            strbAnswers.AppendFormat("{0}->{1}\n", System.DateTime.Now.ToString("HH:mm:ss:fff"), content.Text);
                        }
                    }

                    this.answerBox.text += strbAnswers.ToString();
                },
                (errorResult) =>
                {
                    this.answerBox.text = errorResult.info;
                });
        }
        public void Clear()
        {
            this.questionBox.text = "";
            this.answerBox.text = "";
        }
    }
}
