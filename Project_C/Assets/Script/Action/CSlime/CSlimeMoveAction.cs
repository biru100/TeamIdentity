using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CSlimeMoveAction : CharacterAction
{

public static CSlimeMoveAction GetInstance() { return new CSlimeMoveAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"run");
NodeUtil.MoveToPlayer(Owner);
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.HitDeadLogicMacro(Owner ,"CSlimeHitAction" ,"CSlimeDeadAction"))
{
}

else
{
NodeUtil.LookPlayer(Owner);
NodeUtil.RotationAnim(Owner ,"run");

if(NodeUtil.PlayerInRange(Owner ,0.7f))
{
NodeUtil.ChangeAction(Owner ,"CSlimeAttackAction");
}

else
{
NodeUtil.MoveToPlayer(Owner);
}
}
}

public override void FinishAction()
{
base.FinishAction();
NodeUtil.StopMovement(Owner);
}
}
