using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseCardsCountFilterModule : DataFilterModule
{
    public Dictionary<int, int> RoomUseCardCountData { get; protected set; }
    public Dictionary<int, int> GlobalUseCardCountData { get; protected set; }
    public int RoomTotalUseCardCount { get; protected set; }
    public int GlobalTotalUseCardCount { get; protected set; }

    public UseCardsCountFilterModule()
    {
        RoomUseCardCountData = new Dictionary<int, int>();
        GlobalUseCardCountData = new Dictionary<int, int>();
    }

    public int GetGlobalUseCardCount(int cardIndex)
    {
        return GlobalUseCardCountData.ContainsKey(cardIndex) ? GlobalUseCardCountData[cardIndex] : 0;
    }

    public int GetRoomUseCardCount(int cardIndex)
    {
        return RoomUseCardCountData.ContainsKey(cardIndex) ? RoomUseCardCountData[cardIndex] : 0;
    }

    public override void InspectData(BaseLogData logData)
    {
        base.InspectData(logData);

        if(logData.Type == BaseLogData.LogType.E_ChangeRoom)
        {
            RoomUseCardCountData.Clear();
            RoomTotalUseCardCount = 0;
        }
        else if(logData.Type == BaseLogData.LogType.E_UseCard)
        {
            UseCardLogData useCardData = logData as UseCardLogData;
            if(!RoomUseCardCountData.ContainsKey(useCardData.CardIndex))
            {
                RoomUseCardCountData.Add(useCardData.CardIndex, 1);
            }
            else
            {
                RoomUseCardCountData[useCardData.CardIndex] += 1;
            }

            if (!GlobalUseCardCountData.ContainsKey(useCardData.CardIndex))
            {
                GlobalUseCardCountData.Add(useCardData.CardIndex, 1);
            }
            else
            {
                GlobalUseCardCountData[useCardData.CardIndex] += 1;
            }

            RoomTotalUseCardCount++;
            GlobalTotalUseCardCount++;
        }
    }
}
