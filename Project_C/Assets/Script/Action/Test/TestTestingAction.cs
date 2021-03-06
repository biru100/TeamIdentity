using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TestTestingAction : CharacterAction
{
    public static TestTestingAction GetInstance() { return new TestTestingAction(); }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        NodeUtil.BurnCard();
        NodeUtil.DrawCard();
        NodeUtil.TakeDamage(Owner, 10f);

        //Owner.Status;
        //Owner.AddState(new CharacterHitState(Owner, 1f).Init());

        //if(NodeUtil.IsGoalDestination(Owner))
        //{
        //    Owner.transform.position = new UnityEngine.Vector3(10f ,0f ,10f);
        //    Owner.Status.CurrentHp = Mathf.Min(NodeUtil.FloatAdd(Owner.Status.CurrentHp ,10f) ,Owner.Status.Hp);
        //}
    }
    
    public override void UpdateAction()
    {
        base.UpdateAction();

        if(NodeUtil.IsGoalDestination(Owner))
        {
            if (NodeUtil.PlayerInRange(Owner, 10f))
            {
                NodeUtil.AvoidFormPlayer(Owner);
            }
        }
    }
    
    public override void FinishAction()
    {
        base.FinishAction();
    }
}
