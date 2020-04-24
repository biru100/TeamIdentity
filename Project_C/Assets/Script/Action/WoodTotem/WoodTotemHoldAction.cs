using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WoodTotemHoldAction : CharacterAction
{

public static WoodTotemHoldAction GetInstance() { return new WoodTotemHoldAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"stun");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacroByCurrentOrder(Owner ,2))
{
}

else
{

if(NodeUtil.StateFinishActionMacro(Owner ,2))
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
