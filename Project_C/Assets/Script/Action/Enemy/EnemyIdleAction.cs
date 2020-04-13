using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyIdleAction : CharacterAction
{

public static EnemyIdleAction GetInstance() { return new EnemyIdleAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"idle");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.HitDeadLogicMacro(Owner ,"EnemyHitAction" , "EnemyIdleAction"))
{
}

else
{
}
}

public override void FinishAction()
{
base.FinishAction();
}
}
