using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinSlingIdleAction : CharacterAction
{

    public static GoblinSlingIdleAction GetInstance() { return new GoblinSlingIdleAction(); }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        NodeUtil.PlayAnim(Owner, "idle");
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (NodeUtil.StateActionMacro(Owner))
        {
        }

        else
        {

            if (NodeUtil.PlayerInRange(Owner, 7f))
            {
                NodeUtil.LookPlayer(Owner);
                NodeUtil.ChangeAction(Owner, "GoblinSlingMoveAction");
            }

            else
            {
            }
        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
    }
}
