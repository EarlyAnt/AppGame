using AppGame.Data.Common;
using AppGame.Data.Model;
using System;
using System.Collections.Generic;

namespace AppGame.Data.Remote
{
    public interface IBotDialogDataUtil
    {
        void PostDialogData(DialogRequest dialogRequest, Action<DialogResponse> callback = null, Action<Result> errCallback = null);
    }

    #region 自定义类
    #region 公共类
    public class StringProps
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }

    public class ObjectProps
    {
        public object additionalProp1 { get; set; }
        public object additionalProp2 { get; set; }
        public object additionalProp3 { get; set; }
    }
    #endregion
    #region Request相关
    public class DialogRequest
    {
        public string sessionId { get; set; }
        public string actionAreaId { get; set; }
        public string channelId { get; set; }
        public DialogInput input { get; set; }
        public StringProps sessionMeta { get; set; }
    }

    public class DialogInput
    {
        public QueryRequest query { get; set; }
        //public Event event { get; set; }
    }

    public class QueryRequest
    {
        public string queryText { get; set; }
    }

    public class Event
    {
        public string name { get; set; }
        public ObjectProps parameters { get; set; }
    }
    #endregion
    #region Response相关
    public class DialogResponse : DataBase
    {
        public string sessionId { get; set; }
        public QueryResponse queryResult { get; set; }
        public WebhookStatus webhookStatus { get; set; }
    }

    public class QueryResponse
    {
        public string agentId { get; set; }
        public string actionAreaId { get; set; }
        public string channelId { get; set; }
        public string queryText { get; set; }
        public string eventName { get; set; }
        public string actionName { get; set; }
        public ObjectProps parameters { get; set; }
        public List<ResponseText> responseText { get; set; }
        public bool isDoYouMean { get; set; }
        public bool isFallBack { get; set; }
        public bool isFAQ { get; set; }
        public bool isNoChannelContent { get; set; }
        public bool isSlotFilling { get; set; }
        public bool isBadRequest { get; set; }
        public bool isSensitiveWords { get; set; }
        public StringProps sessionMeta { get; set; }
        public List<OutputContext> outputContexts { get; set; }
        public Intent intent { get; set; }
        public List<Entity> entities { get; set; }
        public List<AlternativeAnswer> alternativeAnswers { get; set; }
    }

    public class ResponseText
    {
        public class Content
        {
            public string LeadingMessage { get; set; }
            public string EndingMessage { get; set; }
            public string Label { get; set; }
            public string Text { get; set; }
            public List<string> Options { get; set; }
        }

        public string type { get; set; }
        public string content { get; set; }
    } 

    public class OutputContext
    {
        public string name { get; set; }
        public int lifespanCount { get; set; }
        public ObjectProps parameters { get; set; }
        public List<Option> options { get; set; }
        public TriggerMap triggerMap { get; set; }
        public int contextScope { get; set; }
        public string updateTime { get; set; }
    }

    public class Option
    {
        public string displayText { get; set; }
        public string id { get; set; }
    }

    public class TriggerMap
    {
        
    }

    public class Entity
    {
        public string entityType { get; set; }
        public string entityValue { get; set; }
    }

    public class Intent
    {
        public string id { get; set; }
        public string name { get; set; }
        public float confidence { get; set; }
    }

    public class AlternativeAnswer
    {
        public string id { get; set; }
        public string standardQuestion { get; set; }
        public string answer { get; set; }
        public float confidence { get; set; }
    }

    public class WebhookStatus
    {
        public int statusCode { get; set; }
        public string message { get; set; }
    }
    #endregion
    #endregion
}