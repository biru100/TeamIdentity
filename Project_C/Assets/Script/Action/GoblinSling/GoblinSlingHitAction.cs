using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinSlingHitAction : CharacterAction
{

public static GoblinSlingHitAction GetInstance() { return new GoblinSlingHitAction(); }

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
NodeUtil.ChangeAction(Owner ,"GoblinSlingIdleAction");
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
