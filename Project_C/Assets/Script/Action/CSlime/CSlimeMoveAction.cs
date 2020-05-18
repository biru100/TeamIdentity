using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CSlimeMoveAction : CharacterAction
{

    public static CSlimeMoveAction GetInstance() { return new CSlimeMoveAction(); }

    int timer1, timer2;
    int check1;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        NodeUtil.PlayAnim(Owner, "run");

        timer1 = 0;
        timer2 = UnityEngine.Random.Range(40, 100);
        check1 = UnityEngine.Random.Range(1, 4);

    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (NodeUtil.StateActionMacro(Owner))
        {
        }

        else
        {
            if (check1 == 1)
            {
                timer1 = timer1 + 1;

                if (timer1 == timer2)
                {
                    timer1 = 0;
                    timer2 = UnityEngine.Random.Range(40, 100);
                    check1 = UnityEngine.Random.Range(1, 3);
                    NodeUtil.StopMovement(Owner);
                }
                else
                {
                    NodeUtil.LookPlayer(Owner);
                    NodeUtil.RotationAnim(Owner, "run");

                    if (NodeUtil.PlayerInRange(Owner, 1f))
                    {
                        NodeUtil.ChangeAction(Owner, "CSlimeAttackAction");
                    }

                    else
                    {
                        NodeUtil.MoveToPlayer(Owner);
                    }
                }
            }

            else if(check1 == 2)
            {
                timer1 = timer1 + 1;

                if (timer1 == timer2)
                {
                    timer1 = 0;
                    timer2 = UnityEngine.Random.Range(40, 100);
                    check1 = UnityEngine.Random.Range(1, 3);

                }
                else
                {
                    NodeUtil.LookPlayer(Owner);
                    NodeUtil.RotationAnim(Owner, "idle");
                    NodeUtil.StopMovement(Owner);
                }
            }
            else if(check1 == 3)
            {
                timer1 = timer1 + 1;

                if (timer1 == timer2)
                {
                    timer1 = 0;
                    timer2 = UnityEngine.Random.Range(40, 100);
                    check1 = UnityEngine.Random.Range(1, 3);

                }
                else
                {
                    Owner.transform.rotation = Quaternion.LookRotation((Owner.transform.position- Player.CurrentPlayer.transform.position).normalized , Vector3.up);
                    NodeUtil.RotationAnim(Owner, "run");
                    NodeUtil.AvoidFormPlayer(Owner);
                }

            }

        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
        NodeUtil.StopMovement(Owner);
    }
}
