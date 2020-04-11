using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyIdleAction : CharacterAction
{

public static EnemyIdleAction Instance = new EnemyIdleAction();

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"idle");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.HitDeadLogicMacro(Owner ,"EnemyHitAction" ,""))
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
