using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardPresentIdleAction : CharacterAction
{

public static CardPresentIdleAction GetInstance() { return ObjectPooling.PopObject<CardPresentIdleAction>(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
}

public override void UpdateAction()
{
base.UpdateAction();
        if ((Owner as NPC).IsCalledInteraction && (Owner as NPC).CanUse)
        {
            EntityUtil.ChangeAction(Owner, "CardPresentAbilityAction");
            return;
        }
    }

public override void FinishAction()
{
base.FinishAction();
}
}