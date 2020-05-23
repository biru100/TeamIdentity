using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinHentaiAttackAction : CharacterAction
{

    public static GoblinHentaiAttackAction GetInstance() { return new GoblinHentaiAttackAction(); }

    float SaveCurrentDamage, SaveCurrentSpeed;

    //��ŸƮ �׼�
    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        SaveCurrentDamage = 0;
        SaveCurrentSpeed = 0;
        TimelineEvents.Add(new TimeLineEvent(0.9f, TimeLine_4));
        NodeUtil.PlayAnim(Owner, "attack");
    }

    //������Ʈ �׼�
    public override void UpdateAction()
    {
        base.UpdateAction();

        if (NodeUtil.StateActionMacro(Owner))
        {
        }

        else
        {

            if (NodeUtil.IsLastFrame(Owner))
            {
                NodeUtil.ChangeAction(Owner, "GoblinHentaiIdleAction");
            }

            else
            {
            }
        }
    }

    //�ǴϽ� �׼�
    public override void FinishAction()
    {
        base.FinishAction();
    }

    //�̺�Ʈ
    void TimeLine_4()
    {

        // �÷��̾� ��ġ 2�׸��� 90�� �ȿ� ������ ��!!!
        if (NodeUtil.PlayerInSight(Owner, 2f, 45f))
        {
            //�÷��̾�� �������� �ش�!!!
            if (Owner.StateStack.Count == 0)
            {

            }
            NodeUtil.TakeDamageToPlayer(Owner.Status.CurrentDamage);


            if (NodeUtil.IsActivateAbility(Owner, 214))
            {
                //ī�� ��ο�
                NodeUtil.DrawCard();
                if (Owner.StateStack.Count > 0)
                {
                    SaveCurrentDamage = Owner.Status.CurrentDamage - Owner.Status.Damage;
                    SaveCurrentSpeed = Owner.Status.CurrentSpeed - Owner.Status.Speed;
                    Owner.StateStack.Clear();
                }
    
                    //����� �������� ��!
                    Owner.AddState(new CharacterIncreaseDamageState(Owner, NodeUtil.GetMosterParameter(Owner, 1)+ SaveCurrentDamage, 10f));
                    //����� �̵��ӵ��� ��!
                    Owner.AddState(new CharacterIncreaseSpeedState(Owner, NodeUtil.GetMosterParameter(Owner, 2) + SaveCurrentSpeed, 10f));

   
            }

            //if(NodeUtil.IsActivateAbility(Owner ,201))
            //           {
            //                              Owner.AddState(new CharacterIncreaseDamageState(Owner, NodeUtil.GetMosterParameter(Owner, 1), -1f));
            //         }
            //if(NodeUtil.IsActivateAbility(Owner, 203))
            //           {
            //             Owner.AddState(new CharacterIncreaseSpeedState(Owner, NodeUtil.GetMosterParameter(Owner, 2), -1f));
            //        }
            else
            {
            }
        }

        else
        {
        }
    }
}
