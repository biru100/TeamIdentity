using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerHitAction : CharacterAction
{
    public static PlayerHitAction GetInstance() { return ObjectPooling.PopObject<PlayerHitAction>(); }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        NodeUtil.PlayAnim(Owner ,"hit");
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (NodeUtil.StateActionMacroByCurrentOrder(Owner, 6))
            return;

        if(NodeUtil.IsLastFrame(Owner))
        {
            NodeUtil.ChangeAction(Owner ,"PlayerIdleAction");
        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
    }
}
