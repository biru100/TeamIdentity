using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoneAction : PlayerCardAction
{
    public static PlayerNoneAction GetInstance(CardTable dataTable, TargetData target) { return new PlayerNoneAction(dataTable, target); }

    public PlayerNoneAction(CardTable dataTable, TargetData target) : base(dataTable, target)
    {
    }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        owner.transform.rotation = Quaternion.Euler(0f, 135f, 0f);
        AnimUtil.PlayAnim(owner, "buff");

    }

    public override void UpdateAction()
    {
        base.UpdateAction();
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

 }
