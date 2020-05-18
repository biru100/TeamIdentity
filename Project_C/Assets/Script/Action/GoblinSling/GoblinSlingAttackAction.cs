using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinSlingAttackAction : CharacterAction
{
    Vector3 des;

    public static GoblinSlingAttackAction GetInstance() { return new GoblinSlingAttackAction(); }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        TimelineEvents.Add(new TimeLineEvent(0.6f, TimeLine_4));
        NodeUtil.PlayAnim(Owner, "attack");
        des = Player.CurrentPlayer.transform.position;
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
                NodeUtil.ChangeAction(Owner, "GoblinSlingIdleAction");
            }

            else
            {
                //NodeUtil.LookPlayer(Owner);
            }
        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
    }

    void TimeLine_4()
    {
        NodeUtil.ShootProjectile(Owner, "Bullet", NodeUtil.VectorMinus(des, Owner.transform.position).normalized, 20f);
    }
}
