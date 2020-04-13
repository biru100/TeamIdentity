using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CSlimeDeadAction : CharacterAction
{

public static CSlimeDeadAction GetInstance() { return new CSlimeDeadAction(); }

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
