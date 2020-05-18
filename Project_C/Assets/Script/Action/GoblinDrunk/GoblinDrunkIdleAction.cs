using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinDrunkIdleAction : CharacterAction
{

    public static GoblinDrunkIdleAction GetInstance() { return new GoblinDrunkIdleAction(); }

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
                NodeUtil.ChangeAction(Owner, "GoblinDrunkMoveAction");
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
