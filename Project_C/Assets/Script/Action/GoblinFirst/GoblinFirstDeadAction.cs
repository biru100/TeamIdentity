using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinFirstDeadAction : CharacterAction
{

public static GoblinFirstDeadAction GetInstance() { return new GoblinFirstDeadAction(); }

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
