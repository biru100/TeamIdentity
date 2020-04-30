using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinBuilderIdleAction : CharacterAction
{

public static GoblinBuilderIdleAction GetInstance() { return new GoblinBuilderIdleAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnimOneSide(Owner ,"idle_135");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacro(Owner))
{
}

else
{
}
}

public override void FinishAction()
{
base.FinishAction();
}
}
