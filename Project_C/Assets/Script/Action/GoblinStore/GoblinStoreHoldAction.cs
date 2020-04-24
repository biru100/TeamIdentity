using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinStoreHoldAction : CharacterAction
{

public static GoblinStoreHoldAction GetInstance() { return new GoblinStoreHoldAction(); }

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
