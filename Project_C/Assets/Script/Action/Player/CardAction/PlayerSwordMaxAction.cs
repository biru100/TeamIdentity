using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordMaxAction : PlayerCardAction
{
    public static PlayerSwordMaxAction GetInstance(CardTable dataTable, TargetData target)
    { return ObjectPooling.PopObject<PlayerSwordMaxAction>().SetData(dataTable, target) as PlayerSwordMaxAction; }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        owner.transform.rotation = Quaternion.Euler(0f, 135f, 0f);
        AnimUtil.PlayAnim(owner, "buff");
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
        for (int i = 0; i < InGameInterface.Instance.MaxHandCardCount - InGameInterface.Instance.CurrentHandCardCount; i++)
        {
            CardInterface ci = CardInterface.CreateCard(InGameInterface.Instance.transform);
            ci.CardData = new Card(320);
            ci.CurrentAction = HandCardAction.GetInstance();
            InGameInterface.Instance.AddToHand(ci);

        }
    }
}
