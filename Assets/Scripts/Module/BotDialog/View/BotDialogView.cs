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
        /************************************************�������������************************************************/
        #region ע��ӿ�
        [Inject]
        public IJsonUtil JsonUtil { get; set; }
        [Inject]
        public IBotDialogDataUtil BotDialogDataUtil { get; set; }
        #endregion
        #region ҳ��UI���
        [SerializeField]
        private InputField questionBox;
        [SerializeField]
        private InputField answerBox;
        #endregion
        #region ��������
        private string sessionId = "";
        #endregion
        /************************************************Unity�������¼�***********************************************/
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
        /************************************************�� �� �� �� ��************************************************/
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
