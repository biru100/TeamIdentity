using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinStoreMoveAction : CharacterAction
{

public static GoblinStoreMoveAction GetInstance() { return new GoblinStoreMoveAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"run");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacro(Owner))
{
}

else
{
NodeUtil.RotationAnim(Owner ,"run");
}
}

public override void FinishAction()
{
base.FinishAction();
NodeUtil.StopMovement(Owner);
}
}
