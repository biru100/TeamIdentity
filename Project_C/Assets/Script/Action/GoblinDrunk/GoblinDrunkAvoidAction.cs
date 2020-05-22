using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinDrunkAvoidAction : CharacterAction
{

    public static GoblinDrunkAvoidAction GetInstance() { return new GoblinDrunkAvoidAction(); }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        NodeUtil.PlayAnim(Owner, "run");
        NodeUtil.AvoidFormPlayer(owner);
        Owner.AddState(new CharacterIncreaseSpeedState(Owner, Owner.Status.CurrentSpeed * 2, 3f));
        TimelineEvents.Add(new TimeLineEvent(3f, StopRun));
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (isFinish)
            return;

        if (NodeUtil.StateActionMacro(Owner))
        {
        }

        else
        {
            Owner.transform.rotation = Quaternion.LookRotation((Owner.transform.position - Player.CurrentPlayer.transform.position).normalized, Vector3.up);
            NodeUtil.RotationAnim(Owner, "run");
            NodeUtil.AvoidFormPlayer(Owner);
        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
        NodeUtil.StopMovement(Owner);
    }

    bool isFinish = false;

    public void StopRun()
    {
        NodeUtil.ChangeAction(Owner, "GoblinDrunkIdleAction");
        isFinish = true;
    }
}
