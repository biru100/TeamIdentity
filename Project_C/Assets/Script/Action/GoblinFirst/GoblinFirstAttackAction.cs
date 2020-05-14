using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinFirstAttackAction : CharacterAction
{

    public static GoblinFirstAttackAction GetInstance() { return new GoblinFirstAttackAction(); }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        TimelineEvents.Add(new TimeLineEvent(0.1f, TimeLine_4));
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
                NodeUtil.ChangeAction(Owner, "GoblinFirstIdleAction");
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

        if (NodeUtil.PlayerInSight(Owner, 2f, 65f))
        {
            NodeUtil.TakeDamageToPlayer(20f);

            if (NodeUtil.IsActivateAbility(Owner, 208))
            {
                foreach (var e in NodeUtil.GetCharactersInRange(Owner, false, true, 30f))
                {
                    e.AddState(new CharacterState(CharacterStateType.E_Invincibility, e, NodeUtil.GetMosterParameter(Owner, 0)).Init());
                }

            }

            else
            {
            }
        }

        else
        {
        }
    }
}
