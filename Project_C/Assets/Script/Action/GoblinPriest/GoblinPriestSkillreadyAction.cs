using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinPriestSkillreadyAction : CharacterAction
{

public static GoblinPriestSkillreadyAction GetInstance() { return new GoblinPriestSkillreadyAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
TimelineEvents.Add(new TimeLineEvent(1f, TimeLine_2));
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

if(NodeUtil.IsActivateAbility(Owner ,210))
{
}

else
{
NodeUtil.ChangeAction(Owner ,"GoblinPriestIdleAction");
}
}
}

public override void FinishAction()
{
base.FinishAction();
}

void TimeLine_2()
{
NodeUtil.ChangeAction(Owner ,"GoblinPriestSkillAction");
}
}
