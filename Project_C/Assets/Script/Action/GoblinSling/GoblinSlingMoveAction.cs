using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinSlingMoveAction : CharacterAction
{

    int pandan;
    float timer1, timer2;
    int min;
    int max;
    int chamgo;
    float xx, zz;
    Vector3 DesPos;

    public static GoblinSlingMoveAction GetInstance() { return new GoblinSlingMoveAction(); }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        NodeUtil.PlayAnim(Owner, "run");
        min = 1;
        max = 7;
        chamgo = 0;

        pandan = UnityEngine.Random.Range(min, max);
        timer1 = timer1 = UnityEngine.Random.Range(6, 10);
        timer2 = 0.00f;
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (NodeUtil.StateActionMacro(Owner))
        {
        }

        else
        {
            // 5M 이내로 진입할때 까지 movetoplayer
            if (NodeUtil.PlayerInRange(Owner, 5))
            {


                if (pandan == 1 || pandan == 6)//공격
                {
                    NodeUtil.LookPlayer(Owner);
                    NodeUtil.ChangeAction(Owner, "GoblinSlingAttackAction");
                }
                else if (pandan == 2)//이동->플레이어
                {
                    timer2 += 0.1f;

                    if (timer2 <= timer1) // 2m내에 있으면 판단 변수 재지정 아니면 이동
                    {
                        NodeUtil.LookPlayer(Owner);
                        NodeUtil.RotationAnim(Owner, "run");
                        NodeUtil.MoveToPlayer(Owner);
                    }
                    else
                    {
                        pandan = UnityEngine.Random.Range(min, max);
                        timer2 = 0f;
                        timer1 = UnityEngine.Random.Range(6, 10); ;
                    }


                }
                else if (pandan == 3)//대기
                {
                    NodeUtil.StopMovement(Owner);
                    timer2 += 0.1f;

                    if (timer2 <= timer1)
                    {
                        NodeUtil.LookPlayer(Owner);
                        NodeUtil.RotationAnim(Owner, "idle");
                    }
                    else
                    {
                        pandan = UnityEngine.Random.Range(min, max);
                        timer2 = 0f;
                        timer1 = UnityEngine.Random.Range(6, 10);
                    }

                }
                else if (pandan == 4 || pandan == 5)//랜덤 장소로 이동
                {
                    timer2 += 0.05f;

                    if (timer2 <= timer1)
                    {
                        
                        NodeUtil.RotationAnim(Owner, "run");
                        if (chamgo == 0)
                        {
                            chamgo = 1;
                            xx= UnityEngine.Random.Range(-1f, 1f);
                            zz = UnityEngine.Random.Range(-1f, 1f);
                            DesPos = Owner.transform.position + new Vector3(xx, 0, zz);
                            Owner.transform.rotation = Quaternion.LookRotation((DesPos - Owner.transform.position).normalized, Vector3.up);
                        }
                        DesPos = Owner.transform.position + new Vector3(xx, 0, zz);
                        NodeUtil.MoveToPpsition(Owner, DesPos);


                    }
                    else
                    {
                        pandan = UnityEngine.Random.Range(min, max);
                        timer2 = 0f;
                        chamgo = 0;
                        timer1 = UnityEngine.Random.Range(6, 10); 
                    }
                }
                else //도망
                {
                    timer2 += 0.1f;

                    if (timer2 <= timer1) // 7m내에 PC가 없으면 재지저
                    {
                        Owner.transform.rotation = Quaternion.LookRotation((Owner.transform.position- Player.CurrentPlayer.transform.position).normalized, Vector3.up);
                        NodeUtil.RotationAnim(Owner, "run");
                        NodeUtil.AvoidFormPlayer(Owner);
                    }
                    else
                    {
                        pandan = UnityEngine.Random.Range(min, max);
                        timer2 = 0f;
                        timer1 = UnityEngine.Random.Range(6, 10);
                    }
                }

            }
            else
            {
                NodeUtil.LookPlayer(Owner);
                NodeUtil.RotationAnim(Owner, "run");
                NodeUtil.MoveToPlayer(Owner);
            }
        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
        NodeUtil.StopMovement(Owner);
    }
}
