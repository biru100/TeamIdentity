using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinHentaiAttackAction : CharacterAction
{

    public static GoblinHentaiAttackAction GetInstance() { return new GoblinHentaiAttackAction(); }

    float SaveCurrentDamage, SaveCurrentSpeed;

    //스타트 액션
    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        SaveCurrentDamage = 0;
        SaveCurrentSpeed = 0;
        TimelineEvents.Add(new TimeLineEvent(0.9f, TimeLine_4));
        NodeUtil.PlayAnim(Owner, "attack");
    }

    //업데이트 액션
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

    //피니시 액션
    public override void FinishAction()
    {
        base.FinishAction();
    }

    //이벤트
    void TimeLine_4()
    {

        // 플레이어 위치 2그리드 90도 안에 존재할 시!!!
        if (NodeUtil.PlayerInSight(Owner, 2f, 45f))
        {
            //플레이어에게 데미지를 준다!!!
            if (Owner.StateStack.Count == 0)
            {

            }
            NodeUtil.TakeDamageToPlayer(Owner.Status.CurrentDamage);


            if (NodeUtil.IsActivateAbility(Owner, 214))
            {
                //카드 드로우
                NodeUtil.DrawCard();
                if (Owner.StateStack.Count > 0)
                {
                    SaveCurrentDamage = Owner.Status.CurrentDamage - Owner.Status.Damage;
                    SaveCurrentSpeed = Owner.Status.CurrentSpeed - Owner.Status.Speed;
                    Owner.StateStack.Clear();
                }
    
                    //고블린의 데미지를 업!
                    Owner.AddState(new CharacterIncreaseDamageState(Owner, NodeUtil.GetMosterParameter(Owner, 1)+ SaveCurrentDamage, 10f));
                    //고블린의 이동속도를 업!
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
