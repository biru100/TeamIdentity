using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinStealerAvoidAction : CharacterAction
{

public static GoblinStealerAvoidAction GetInstance() { return new GoblinStealerAvoidAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"run");
NodeUtil.MoveToPlayer(Owner);
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacro(Owner))
{
}

else
{
NodeUtil.AvoidFormPlayer(Owner);
NodeUtil.RotationAnim(Owner ,"run");

if(NodeUtil.PlayerInRange(Owner ,3f))
{
NodeUtil.AvoidFormPlayer(Owner);
}

else
{
NodeUtil.ChangeAction(Owner , "GoblinStealerIdleAction");
}
}
}

public override void FinishAction()
{
base.FinishAction();
NodeUtil.StopMovement(Owner);
}
}
