using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HealTotemAbilityAction : CharacterAction
{

    public static HealTotemAbilityAction GetInstance() { return ObjectPooling.PopObject<HealTotemAbilityAction>(); }
    
    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        (Owner as NPC).IsCalledInteraction = false;
        TimelineEvents.Add(new TimeLineEvent(0.2f, AbilityLogic));
    }
    
    public override void UpdateAction()
    {
        base.UpdateAction();
    }
    
    public override void FinishAction()
    {
        base.FinishAction();
    }

    void AbilityLogic()
    {
        PlayerStatus.CurrentStatus.CurrentHp += (Owner as NPC).Data._Parameter[0];
        PlayerStatus.CurrentStatus.CurrentHp = Mathf.Min(PlayerStatus.CurrentStatus.CurrentHp, PlayerStatus.CurrentStatus.Hp);

        (Owner as NPC).IsUse = true;

        EntityUtil.ChangeAction(Owner, "HealTotemIdleAction");
    }
}
