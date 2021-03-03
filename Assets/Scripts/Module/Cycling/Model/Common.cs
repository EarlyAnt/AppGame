using System;
using System.Collections.Generic;

namespace AppGame.Module.Cycling
{
    #region 自定义枚举
    public enum GameEvent
    {
        //普通事件
        GAME_START,
        GAME_OVER,
        COLLECT_MP,
        GO_CLICK,
        MOVE_FORWARD,
        INTERACTION,


        //页面关闭事件
        SCENIC_CARD_CLOSE,
        CITY_STATION_CLOSE
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
    public class MpData
    {
        public MpBallTypes MpBallType { get; set; }
        public string FromID { get; set; }
        public string FromName { get; set; }
        public int Value { get; set; }
    }

    public class Ticket
    {
        public bool Go { get; set; }//true-跳转到下一地图，false-留在原地
        public string FromMapID { get; set; }
        public string ToMapID { get; set; }
        public int Coin { get; set; }
        public int Step { get; set; }
    }
    #endregion
}

