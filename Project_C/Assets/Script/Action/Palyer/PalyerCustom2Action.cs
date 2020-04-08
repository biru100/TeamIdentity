using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PalyerCustom2Action : CharacterAction
{

public static PalyerCustom2Action Instance = new PalyerCustom2Action();

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"run");
}

public override void UpdateAction()
{
base.UpdateAction();
NodeUtil.Move(Owner ,NodeUtil.VectorMultiple(NodeUtil.GetVelocityInput() ,NodeUtil.FloatMultiple(NodeUtil.GetDeltaTime() ,1.2f)));
NodeUtil.RotateToVelocity(Owner ,NodeUtil.GetVelocityInput());
NodeUtil.RotationAnim(Owner ,"run");
}

public override void FinishAction()
{
base.FinishAction();
}
}
