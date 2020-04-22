using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SlimeIdleAction : CharacterAction
{

public static SlimeIdleAction GetInstance() { return new SlimeIdleAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"idle");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacro(Owner))
{
}

else
{

if(NodeUtil.PlayerInRange(Owner ,7f))
{
NodeUtil.ChangeAction(Owner ,"SlimeMoveAction");
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
