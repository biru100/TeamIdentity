using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinBuilderHitAction : CharacterAction
{

public static GoblinBuilderHitAction GetInstance() { return new GoblinBuilderHitAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"hit");
}

public override void UpdateAction()
{
base.UpdateAction();

if(false)
{
}

else
{

if(NodeUtil.IsLastFrame(Owner))
{
NodeUtil.ChangeAction(Owner ,"GoblinBuilderIdleAction");
}

else
{
}
}
}

public override void FinishAction()
{
base.FinishAction();
}
}
