using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DruidMoveAction : CharacterAction
{

public static DruidMoveAction GetInstance() { return new DruidMoveAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"run");
NodeUtil.AvoidFormPlayer(Owner);
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacro(Owner))
{
}

else
{
NodeUtil.LookPlayer(Owner);
NodeUtil.RotationAnim(Owner ,"run");

if(NodeUtil.PlayerInRange(Owner ,8f))
{

if(NodeUtil.PlayerInRange(Owner ,5f))
{
NodeUtil.ChangeAction(Owner ,"DruidAttackAction");
NodeUtil.AvoidFormPlayer(Owner);
}

else
{
}
}

else
{
NodeUtil.ChangeAction(Owner ,"DruidIdleAction");
}
}
}

public override void FinishAction()
{
base.FinishAction();
NodeUtil.StopMovement(Owner);
}
}
