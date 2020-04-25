using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SlimeHitAction : CharacterAction
{

public static SlimeHitAction GetInstance() { return new SlimeHitAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.LookPlayer(Owner);
NodeUtil.PlayAnim(Owner ,"hit");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacroByCurrentOrder(Owner ,6))
{
}

else
{

if(NodeUtil.IsLastFrame(Owner))
{
NodeUtil.ChangeAction(Owner ,"SlimeIdleAction");
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
