using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinStealerAttackAction : CharacterAction
{

    public static GoblinStealerAttackAction GetInstance() { return new GoblinStealerAttackAction(); }

    Vector3 PlayerPosition;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        NodeUtil.PlayAnim(Owner, "attack");
        PlayerPosition = Player.CurrentPlayer.transform.position ;
        TimelineEvents.Add(new TimeLineEvent(0.6f, TimeLine_4));

    }

    public override void UpdateAction()
    {
        base.UpdateAction();
        

        if (NodeUtil.StateActionMacro(Owner))
        {
        }

        else
        {
            Owner.Status.CurrentSpeed = 20f;

            if (NodeUtil.IsLastFrame(Owner))
            {
                Owner.Status.CurrentSpeed = Owner.Status.Speed;
                NodeUtil.StopMovement(Owner);
                NodeUtil.ChangeAction(Owner, "GoblinStealerAvoidAction");
                return;
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

        if (NodeUtil.PlayerInSight(Owner, 6f, 15f))
        {
            NodeUtil.TakeDamageToPlayer(Owner.Status.CurrentDamage);

            if (NodeUtil.IsActivateAbility(Owner, 215))
            {
                NodeUtil.GiveSilence(Player.CurrentPlayer, 2f);
                NodeUtil.BurnCard();
            }

            //if (NodeUtil.IsActivateAbility(Owner, 207))
            //{
            //    NodeUtil.GiveSilence(Player.CurrentPlayer, 2f);
            //}
            else
            {

            }
        }
        else
        {
        }

        Owner.NavAgent.Move(Owner.transform.forward*2);
    }
}
