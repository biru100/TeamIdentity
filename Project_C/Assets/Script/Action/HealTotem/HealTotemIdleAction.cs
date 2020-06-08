using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HealTotemIdleAction : CharacterAction
{

public static HealTotemIdleAction GetInstance() { return ObjectPooling.PopObject<HealTotemIdleAction>(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
}

public override void UpdateAction()
{
base.UpdateAction();

        if((Owner as NPC).IsCalledInteraction && (Owner as NPC).CanUse)
        {
            EntityUtil.ChangeAction(Owner, "HealTotemAbilityAction");
            return;
        }
}

public override void FinishAction()
{
base.FinishAction();
}
}
