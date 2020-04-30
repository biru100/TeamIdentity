using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinBuilderDeadAction : CharacterAction
{

public static GoblinBuilderDeadAction GetInstance() { return new GoblinBuilderDeadAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"die");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.IsLastFrame(Owner))
{

if(NodeUtil.IsActivateAbility(Owner ,211))
{
NodeUtil.ChangeAction(NodeUtil.CreateEntity("GuardianStone" ,NodeUtil.GetPosition(Owner)) ,"GuardianStoneCreateAction");
NodeUtil.DestroyEntity(Owner);
}

else
{
NodeUtil.DestroyEntity(Owner);
}
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
