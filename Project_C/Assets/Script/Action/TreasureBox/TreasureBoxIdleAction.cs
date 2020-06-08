using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TreasureBoxIdleAction : CharacterAction
{

public static TreasureBoxIdleAction GetInstance() { return ObjectPooling.PopObject<TreasureBoxIdleAction>(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
}

public override void UpdateAction()
{
base.UpdateAction();
        if ((Owner as NPC).IsCalledInteraction && (Owner as NPC).CanUse)
        {
            EntityUtil.ChangeAction(Owner, "TreasureBoxAbilityAction");
            return;
        }
    }

public override void FinishAction()
{
base.FinishAction();
}
}
