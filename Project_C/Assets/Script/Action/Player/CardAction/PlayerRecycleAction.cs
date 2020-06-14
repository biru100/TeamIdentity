using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecycleAction : PlayerCardAction
{
    public static PlayerRecycleAction GetInstance(CardTable dataTable, TargetData target)
    { return ObjectPooling.PopObject<PlayerRecycleAction>().SetData(dataTable, target) as PlayerRecycleAction; }

    int UseCost;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        owner.transform.rotation = Quaternion.Euler(0f, 135f, 0f);
        AnimUtil.PlayAnim(owner, "buff");

        UseCost = PlayingDataManager.GetModule<UseCostFilterModule>().RoomUseCost;

        TimelineEvents.Add(new TimeLineEvent(0.1f, AddBuff));
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (EntityUtil.StateActionMacro(Owner, CharacterStateType.E_Hold))
        {
            return;
        }

        if (AnimUtil.IsLastFrame(Owner))
        {
            Owner.CurrentAction = PlayerIdleAction.GetInstance();
            return;

        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
    }

    public void AddBuff()
    {
        PlayerStatus.CurrentStatus.CurrentManaCost += UseCost/2;
    }
}
