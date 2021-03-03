using System;
using System.Collections.Generic;

namespace AppGame.Module.Cycling
{
    #region �Զ���ö��
    public enum GameEvent
    {
        //��ͨ�¼�
        GAME_START,
        GAME_OVER,
        COLLECT_MP,
        GO_CLICK,
        MOVE_FORWARD,
        INTERACTION,


        //ҳ��ر��¼�
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

    #region �Զ�����
    public class MpData
    {
        public MpBallTypes MpBallType { get; set; }
        public string FromID { get; set; }
        public string FromName { get; set; }
        public int Value { get; set; }
    }

    public class Ticket
    {
        public bool Go { get; set; }//true-��ת����һ��ͼ��false-����ԭ��
        public string FromMapID { get; set; }
        public string ToMapID { get; set; }
        public int Coin { get; set; }
        public int Step { get; set; }
    }
    #endregion
}

