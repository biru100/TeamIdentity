using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DruidSkillAction : CharacterAction
{

public static DruidSkillAction GetInstance() { return new DruidSkillAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
TimelineEvents.Add(new TimeLineEvent(0.5f, TimeLine_2));
NodeUtil.PlayAnim(Owner ,"run");
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

void TimeLine_2()
{
NodeUtil.TakeDamageBoth(NodeUtil.GetCharactersInRange(Owner ,false ,false ,5f) ,-20f);
NodeUtil.ChangeAction(Owner ,"DruidIdlection");
}
}
