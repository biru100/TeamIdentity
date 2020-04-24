using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinStealerStunAction : CharacterAction
{

public static GoblinStealerStunAction GetInstance() { return new GoblinStealerStunAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"stun");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacroByCurrentOrder(Owner ,1))
{
}

else
{

if(NodeUtil.StateFinishActionMacro(Owner ,1))
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
