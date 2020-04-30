using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinBuilderHoldAction : CharacterAction
{

public static GoblinBuilderHoldAction GetInstance() { return new GoblinBuilderHoldAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"hold");
}

public override void UpdateAction()
{
base.UpdateAction();

if(false)
{
}

else
{

if(NodeUtil.StateFinishActionMacro(Owner ,4))
{
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
