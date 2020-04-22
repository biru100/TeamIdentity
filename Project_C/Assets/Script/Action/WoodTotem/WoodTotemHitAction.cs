using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WoodTotemHitAction : CharacterAction
{

public static WoodTotemHitAction GetInstance() { return new WoodTotemHitAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.LookPlayer(Owner);
NodeUtil.PlayAnim(Owner ,"hit");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacroByCurrentOrder(Owner ,3))
{
}

else
{

if(NodeUtil.IsLastFrame(Owner))
{
NodeUtil.ChangeAction(Owner ,"WoodTotemIdleAction");
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
