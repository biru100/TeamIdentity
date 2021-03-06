using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinFirstMoveAction : CharacterAction
{

public static GoblinFirstMoveAction GetInstance() { return new GoblinFirstMoveAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"run");
NodeUtil.MoveToPlayer(Owner);
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacro(Owner))
{
}

else
{
NodeUtil.LookPlayer(Owner);
NodeUtil.RotationAnim(Owner ,"run");

if(NodeUtil.PlayerInRange(Owner ,2f))
{
NodeUtil.ChangeAction(Owner ,"GoblinFirstAttackAction");
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
