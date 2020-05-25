using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinStoreMoveAction : CharacterAction
{

    public static GoblinStoreMoveAction GetInstance() { return new GoblinStoreMoveAction(); }

    bool IsEnd;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        NodeUtil.PlayAnim(Owner, "run");
        TimelineEvents.Add(new TimeLineEvent(3f, End));
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (NodeUtil.StateActionMacro(Owner))
        {
        }

        else
        {
            NodeUtil.AvoidFormPlayer(Owner);
            NodeUtil.RotationAnim(Owner, "run");

            if (!NodeUtil.PlayerInRange(Owner, 5f) || IsEnd)
            {
                NodeUtil.ChangeAction(Owner, "GoblinStoreTiredAction");
            }

        }
    }


    public override void FinishAction()
    {
        base.FinishAction();
        NodeUtil.StopMovement(Owner);
    }

    void End()
    {
        IsEnd = true;
    }
}
