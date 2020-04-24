using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinHentaiIdleAction : CharacterAction
{

public static GoblinHentaiIdleAction GetInstance() { return new GoblinHentaiIdleAction(); }

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
NodeUtil.ChangeAction(Owner ,"GoblinHentaiMoveAction");
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
