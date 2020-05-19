using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinStealerMoveAction : CharacterAction
{

    public static GoblinStealerMoveAction GetInstance() { return new GoblinStealerMoveAction(); }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        NodeUtil.PlayAnim(Owner, "run");
        NodeUtil.MoveToPlayer(Owner);
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (NodeUtil.StateActionMacro(Owner))
        {
        }

        else
        {
            NodeUtil.LookPlayer(Owner);
            NodeUtil.RotationAnim(Owner, "run");

            if (NodeUtil.PlayerInRange(Owner, 1.5f))
            {
                NodeUtil.ChangeAction(Owner, "GoblinStealerAttackAction");
                return;
            }

            else
            {
                NodeUtil.MoveToPlayer(Owner);
            }

            if (NodeUtil.IsLastFrame(Owner))
            {
                NodeUtil.ChangeAction(Owner, "GoblinStoreIdleAction");
            }
        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
        NodeUtil.StopMovement(Owner);
    }
}
