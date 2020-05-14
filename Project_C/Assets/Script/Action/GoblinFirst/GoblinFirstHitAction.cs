using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinFirstHitAction : CharacterAction
{

public static GoblinFirstHitAction GetInstance() { return new GoblinFirstHitAction(); }

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
NodeUtil.ChangeAction(Owner ,"GoblinFirstIdleAction");
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
