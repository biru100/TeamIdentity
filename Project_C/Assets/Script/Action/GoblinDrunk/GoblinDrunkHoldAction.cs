using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinDrunkHoldAction : CharacterAction
{

public static GoblinDrunkHoldAction GetInstance() { return new GoblinDrunkHoldAction(); }

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
