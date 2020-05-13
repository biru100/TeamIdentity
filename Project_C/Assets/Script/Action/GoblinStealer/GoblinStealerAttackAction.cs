using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinStealerAttackAction : CharacterAction
{

    public static GoblinStealerAttackAction GetInstance() { return new GoblinStealerAttackAction(); }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        TimelineEvents.Add(new TimeLineEvent(0.5f, TimeLine_4));
        NodeUtil.PlayAnim(Owner, "attack");
        NodeUtil.MoveToPlayer(Owner);
        Owner.AddState(new CharacterIncreaseSpeedState(Owner, 200f, 0.15f));
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
                NodeUtil.ChangeAction(Owner, "GoblinStealerIdleAction");
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

        if (NodeUtil.PlayerInSight(Owner, 1f, 50f))
        {
            NodeUtil.TakeDamageToPlayer(10f);

            if (NodeUtil.IsActivateAbility(Owner, 215))
            {
                NodeUtil.BurnCard();
            }

            if (NodeUtil.IsActivateAbility(Owner, 207))
            {
                NodeUtil.GiveSilence(Player.CurrentPlayer, 2f);
                NodeUtil.PlayAnim(Owner, "run");
                NodeUtil.AvoidFormPlayer(Owner);
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
