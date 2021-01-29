using System;
using System.Collections.Generic;

namespace AppGame.Module.Cycling
{
    #region 自定义枚举
    public enum GameEvent
    {
        ADD_TO_SCORE,
        GAME_OVER,
        GAME_UPDATE,
        LIVES_CHANGE,
        REMOVE_SOCIAL_CONTEXT,
        REPLAY,
        RESTART_GAME,
        SCORE_CHANGE,
        SHIP_DESTROYED
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
    #endregion

    #region 自定义类
    public class LocationDatas
    {
        public List<LocationData> Datas { get; set; }

        public LocationDatas()
        {
            this.Datas = new List<LocationData>();
        }
    }
    public class LocationData
    {
        public string UserID { get; set; }
        public string AvatarID { get; set; }
        public string MapPointID { get; set; }
    }
    #endregion
}

