using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyIdleAction : CharacterAction
{

public static EnemyIdleAction Instance = new EnemyIdleAction();

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.Move(Owner ,NodeUtil.CreateVector3(10 ,1 ,5));
}

public override void UpdateAction()
{
base.UpdateAction();
}

public override void FinishAction()
{
base.FinishAction();
}
}
