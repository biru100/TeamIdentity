using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinSlingHoldAction : CharacterAction
{

public static GoblinSlingHoldAction GetInstance() { return new GoblinSlingHoldAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"stun");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacroByCurrentOrder(Owner ,4))
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
