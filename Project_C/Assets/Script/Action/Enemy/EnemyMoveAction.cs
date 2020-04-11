using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyMoveAction : CharacterAction
{

public static EnemyMoveAction Instance = new EnemyMoveAction();

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"move");
}

public override void UpdateAction()
{
base.UpdateAction();
NodeUtil.Move(Owner ,NodeUtil.VectorMultiple(NodeUtil.GetVelocityInput() ,NodeUtil.FloatMultiple(NodeUtil.GetDeltaTime() ,1.2f)));
NodeUtil.RotateToVelocity(Owner ,NodeUtil.GetVelocityInput());
NodeUtil.RotationAnim(Owner ,"move");
}

public override void FinishAction()
{
base.FinishAction();
}
}
