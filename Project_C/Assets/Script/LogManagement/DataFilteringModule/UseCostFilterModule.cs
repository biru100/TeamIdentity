using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseCostFilterModule : DataFilterModule
{
    public int RoomUseCost { get; protected set; }
    public int GlobalUseCost { get; protected set; }


    public override void InspectData(BaseLogData logData)
    {
        base.InspectData(logData);

        if (logData.Type == BaseLogData.LogType.E_ChangeRoom)
        {
            RoomUseCost = 0;
        }
        else if (logData.Type == BaseLogData.LogType.E_UseCard)
        {
            UseCardLogData useCardData = logData as UseCardLogData;

            RoomUseCost += useCardData.UseCost;
            GlobalUseCost += useCardData.UseCost;
        }
    }
}
