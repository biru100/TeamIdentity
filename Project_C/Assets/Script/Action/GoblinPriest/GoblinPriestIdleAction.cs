using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinPriestIdleAction : CharacterAction
{

public static GoblinPriestIdleAction GetInstance() { return new GoblinPriestIdleAction(); }

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

if(NodeUtil.IsActivateAbility(Owner ,210))
{
NodeUtil.ChangeAction(Owner ,"GoblinPriestSkillreadyAction");
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
