using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinStoreTiredAction : CharacterAction
{

    public static GoblinStoreTiredAction GetInstance() { return new GoblinStoreTiredAction(); }

    bool IsEnd;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        NodeUtil.PlayAnim(Owner, "idle");
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

            if (IsEnd)
            {
                NodeUtil.ChangeAction(Owner, "GoblinStoreIdleAction");
            }


        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
    }

    void End()
    {
        IsEnd = true;
    }
}
