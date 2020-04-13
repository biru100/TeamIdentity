using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CSlimeIdleAction : CharacterAction
{

public static CSlimeIdleAction GetInstance() { return new CSlimeIdleAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"idle");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.HitDeadLogicMacro(Owner ,"CSlimeHitAction" ,"CSlimeDeadAction"))
{
}

else
{

if(NodeUtil.PlayerInRange(Owner ,7f))
{
NodeUtil.ChangeAction(Owner ,"CSlimeMoveAction");
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
}
