using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinStoreIdleAction : CharacterAction
{

public static GoblinStoreIdleAction GetInstance() { return new GoblinStoreIdleAction(); }

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

if(NodeUtil.PlayerInRange(Owner ,2f))
{
NodeUtil.ChangeAction(Owner ,"GoblinStoreMoveAction");
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
