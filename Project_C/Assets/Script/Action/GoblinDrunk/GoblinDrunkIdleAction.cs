using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinDrunkIdleAction : CharacterAction
{

    public static GoblinDrunkIdleAction GetInstance() { return new GoblinDrunkIdleAction(); }

    int timer1, timer2;
    int check1;
    public bool IsAttack;
    Vector3 RandomMove;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        timer1 = 0;
        timer2 = 200;
        check1 = 2;
        NodeUtil.PlayAnim(Owner, "idle");
        if (NodeUtil.PlayerInRange(Owner, 3f)) IsAttack = true;
        TimelineEvents.Add(new TimeLineEvent(1.5f, DelayEnd));
        RandomMove.Set(UnityEngine.Random.Range(-10, 10),0, UnityEngine.Random.Range(-10, 10));
        RandomMove = RandomMove + Owner.transform.position;
    }

    public override void UpdateAction()
    {
        base.UpdateAction();
        
        timer1 += 1;

        if (NodeUtil.StateActionMacro(Owner))
        {
        }


        else
        {
            if (check1 == 1)
            {
                if (timer1 == timer2)
                {
                    timer1 = 0;
                    timer2 = UnityEngine.Random.Range(80, 120);
                    check1 = UnityEngine.Random.Range(1, 3);
                    RandomMove.Set(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
                    RandomMove = RandomMove + Owner.transform.position;
                    NodeUtil.StopMovement(Owner);
                    NodeUtil.RotationAnim(Owner, "idle");
                }
                else
                {
                    Owner.transform.rotation = Quaternion.LookRotation(RandomMove.normalized, Vector3.up);
                    NodeUtil.RotationAnim(Owner, "run");
                    Owner.NavAgent.destination = RandomMove.normalized;
                    
                }
            }

            else if (check1 == 2)
            {
                timer2 = 200;

                if (timer1 == timer2)
                {
                    timer1 = 0;
                    timer2 = UnityEngine.Random.Range(80, 120);
                    check1 = UnityEngine.Random.Range(1, 3);
                    RandomMove.Set(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
                    RandomMove = RandomMove + Owner.transform.position;

                }
                else
                {
                    NodeUtil.LookPlayer(Owner);
                    NodeUtil.RotationAnim(Owner, "idle");
                    NodeUtil.StopMovement(Owner);
                }
            }
            
            if (IsAttack == false)
            {
                if (NodeUtil.PlayerInRange(Owner, 7f))
                {
                    NodeUtil.ChangeAction(Owner, "GoblinDrunkMoveAction");
                    return;
 
                }
            }
        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
    }

    public void DelayEnd()
    {
        IsAttack = false;
    }

}
