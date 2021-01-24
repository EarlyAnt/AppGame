using System;

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
        EventNode = 1,
        SiteNode = 2,
        EndNode = 3
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

    #endregion
}

