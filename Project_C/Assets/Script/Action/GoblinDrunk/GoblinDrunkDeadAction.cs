using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinDrunkDeadAction : CharacterAction
{

public static GoblinDrunkDeadAction GetInstance() { return new GoblinDrunkDeadAction(); }

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
NodeUtil.DestroyEntity(Owner);
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
