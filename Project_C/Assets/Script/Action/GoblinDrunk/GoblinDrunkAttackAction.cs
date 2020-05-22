using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinDrunkAttackAction : CharacterAction
{

    public static GoblinDrunkAttackAction GetInstance() { return new GoblinDrunkAttackAction(); }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        TimelineEvents.Add(new TimeLineEvent(1f, TimeLine_4));
        NodeUtil.PlayAnim(Owner, "attack");
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (NodeUtil.StateActionMacro(Owner))
        {
        }

        else
        {

            if (NodeUtil.IsLastFrame(Owner))
            {
                
                NodeUtil.ChangeAction(Owner, "GoblinDrunkIdleAction");

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

    void TimeLine_4()
    {

        if (NodeUtil.PlayerInSight(Owner, 1f, 70f))
        {
            NodeUtil.TakeDamageToPlayer(10f);
        }

        else
        {
        }
    }
}
