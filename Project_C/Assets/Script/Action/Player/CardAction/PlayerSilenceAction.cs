﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSilenceAction : PlayerCardAction
{
    public static PlayerSilenceAction GetInstance(CardTable dataTable, TargetData target) { return new PlayerSilenceAction(dataTable, target); }

    public PlayerSilenceAction(CardTable dataTable, TargetData target) : base(dataTable, target)
    {
    }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        owner.transform.rotation = Quaternion.Euler(0f, 135f, 0f);
        AnimUtil.PlayAnim(owner, "buff");
        TimelineEvents.Add(new TimeLineEvent(0.1f, CallSilence));
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

    public void CallSilence()
    {
        Target.Target.AddState(new CharacterSilenceState(Target.Target, DataTable._Parameter[0]));
    }
}
