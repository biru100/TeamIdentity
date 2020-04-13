using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyHitAction : CharacterAction
{

public static EnemyHitAction GetInstance() { return new EnemyHitAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.LookPlayer(Owner);
NodeUtil.PlayAnim(Owner ,"hit");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.HitDeadLogicMacro(Owner ,"EnemyHitAction" ,"EnemyDeadAction"))
{
}

else
{

if(NodeUtil.IsLastFrame(Owner))
{
NodeUtil.ChangeAction(Owner ,"EnemyIdleAction");
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
