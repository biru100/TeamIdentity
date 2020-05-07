using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DruidAttackAction : CharacterAction
{

public static DruidAttackAction GetInstance() { return new DruidAttackAction(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);
TimelineEvents.Add(new TimeLineEvent(0.1f, TimeLine_4));
NodeUtil.PlayAnim(Owner ,"attack");
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
NodeUtil.ChangeAction(Owner ,"DruidIdleAction");
}

else
{
NodeUtil.LookPlayer(Owner);
}
}
}

public override void FinishAction()
{
base.FinishAction();
}

void TimeLine_4()
{
NodeUtil.ShootProjectile(Owner ,"Bullet" ,NodeUtil.VectorMinus(Player.CurrentPlayer.transform.position ,Owner.transform.position).normalized ,12f);
}
}
