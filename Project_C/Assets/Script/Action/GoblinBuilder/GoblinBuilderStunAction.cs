using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinBuilderStunAction : CharacterAction
{

public static GoblinBuilderStunAction GetInstance() { return new GoblinBuilderStunAction(); }

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

if(NodeUtil.StateFinishActionMacro(Owner ,3))
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
