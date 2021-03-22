using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AppGame.Module.Cycling
{
    #region 自定义枚举
    public enum GameEvent
    {
        //普通事件
        GAME_START,
        GAME_OVER,
        MP_CLICK,
        COLLECT_MP,
        GO_CLICK,
        MOVE_FORWARD,
        INTERACTION,

        //页面关闭事件
        SCENIC_CARD_CLOSE,
        CITY_STATION_CLOSE,
        PAY_BILL_CLOSE,
        TRAFFIC_LOADING_CLOSE,

        //其他事件
        SET_TOUCH_PAD_ENABLE
    }

    public enum NodeTypes
    {
        StartNode = 0,
        EmptyNode = 1,
        EventNode = 2,
        SiteNode = 3,
        EndNode = 4
    }

    public enum Interactions
    {
        KNOWLEDGE_FOOD = 0,
        KNOWLEDGE_LANDMARK = 1,
        PROPS_TREASURE_BOX = 2,
        CARD_TRANSFER_CITY = 3,
        CARD_TRANSFER_PROVINCE = 4,
        FRAGMENT_CLOTH = 5,
        FRAGMENT_FURNITURE = 6
    }

    public enum MpBallTypes
    {
        Walk = 0,
        Ride = 1,
        Train = 2,
        Learn = 3,
        Family = 4,
        Friend = 5
    }
    #endregion

    #region 自定义类
    public static class SpineParameters
    {
        public const string MATERIAL_NAME = "SkeletonGraphicDefault";
    }

    public class MpData
    {
        public MpBallTypes MpBallType { get; set; }
        public string FromID { get; set; }
        public string FromName { get; set; }
        public int Mp { get; set; }
        public int Coin { get; set; }
        public bool CoinEnough { get; set; }
        public bool RefreshView { get; set; }
    }

    public static class Vehicles
    {
        public const string BUS = "bus";
        public const string TRAIN = "train";
        public const string CRH = "hsr";
        public const string PLANE = "plane";
    }

    public class Ticket
    {
        public bool Go { get; set; }//true-跳转到下一地图，false-留在原地
        public string FromMapID { get; set; }
        public string FromCityName { get; set; }
        public string FromCityPinYin { get; set; }
        public string ToMapID { get; set; }
        public string ToCityName { get; set; }
        public string ToCityPinYin { get; set; }
        public string Vehicle { get; set; }//交通工具
        public int Coin { get; set; }
        public int Hp { get; set; }
    }
    #endregion
}

