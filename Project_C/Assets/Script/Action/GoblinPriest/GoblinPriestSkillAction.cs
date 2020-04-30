using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinPriestSkillAction : CharacterAction
{

public static GoblinPriestSkillAction GetInstance() { return new GoblinPriestSkillAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
NodeUtil.PlayAnim(Owner ,"heal");
}

public override void UpdateAction()
{
base.UpdateAction();

if(NodeUtil.StateActionMacro(Owner))
{
}

else
{

if(NodeUtil.IsLastFrame(Owner))
{
NodeUtil.TakeDamageBoth(NodeUtil.GetCharactersInRange(Owner ,false ,false ,300f) ,-20f);
NodeUtil.ChangeAction(Owner ,"GoblinPriestIdleAction");
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
