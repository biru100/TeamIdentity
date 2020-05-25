using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinFirstStunAction : CharacterAction
{

public static GoblinFirstStunAction GetInstance() { return new GoblinFirstStunAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"stun");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacroByCurrentOrder(Owner ,3))
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
