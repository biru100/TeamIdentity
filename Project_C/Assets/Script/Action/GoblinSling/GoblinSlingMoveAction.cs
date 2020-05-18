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
            // 5M �̳��� �����Ҷ� ���� movetoplayer
            if (NodeUtil.PlayerInRange(Owner, 5))
            {


                if (pandan == 1 || pandan == 6)//����
                {
                    NodeUtil.LookPlayer(Owner);
                    NodeUtil.ChangeAction(Owner, "GoblinSlingAttackAction");
                }
                else if (pandan == 2)//�̵�->�÷��̾�
                {
                    timer2 += 0.1f;

                    if (timer2 <= timer1) // 2m���� ������ �Ǵ� ���� ������ �ƴϸ� �̵�
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
                else if (pandan == 3)//���
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
                else if (pandan == 4 || pandan == 5)//���� ��ҷ� �̵�
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
                else //����
                {
                    timer2 += 0.1f;

                    if (timer2 <= timer1) // 7m���� PC�� ������ ������
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
