using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerCustomAction : CharacterAction
{

public static PlayerCustomAction Instance = new PlayerCustomAction();

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"attack");
}

public override void UpdateAction()
{
base.UpdateAction();

if(true)
{
NodeUtil.Move(Owner ,NodeUtil.VectorMultiple(NodeUtil.GetVelocityInput() ,NodeUtil.FloatMultiple(NodeUtil.GetDeltaTime() ,0.1f)));
NodeUtil.RotateToVelocity(Owner ,NodeUtil.GetVelocityInput());
NodeUtil.RotationAnim(Owner ,"attack");
}

else
{
NodeUtil.RotationAnim(Owner ,"attack");
}
}

public override void FinishAction()
{
base.FinishAction();
}
}
