using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordRewardAction : PlayerCardAction
{
    public static PlayerSwordRewardAction GetInstance(CardTable dataTable, TargetData target)
    { return ObjectPooling.PopObject<PlayerSwordRewardAction>().SetData(dataTable, target) as PlayerSwordRewardAction; }

    int CardUseCount;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        owner.transform.rotation = Quaternion.Euler(0f, 135f, 0f);
        AnimUtil.PlayAnim(owner, "buff");

        CardUseCount = PlayingDataManager.GetModule<UseCardsCountFilterModule>().GetRoomUseCardCount(320);


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
        for(int i = 0;i < CardUseCount; i++)
        {
            DeckManager.Instance.CurrentDeck.AddCard(new Card(Random.Range(301,330)));
        }
    }
}
