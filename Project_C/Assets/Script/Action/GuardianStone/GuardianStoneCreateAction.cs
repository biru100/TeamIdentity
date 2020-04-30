using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GuardianStoneCreateAction : CharacterAction
{

public static GuardianStoneCreateAction GetInstance() { return new GuardianStoneCreateAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnimOneSide(Owner ,"idle_135");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.IsLastFrame(Owner))
{
NodeUtil.ChangeAction(Owner ,"GuardianStoneIdleAction");
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
