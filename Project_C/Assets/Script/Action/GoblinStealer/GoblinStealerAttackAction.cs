using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoblinStealerAttackAction : CharacterAction
{

    public static GoblinStealerAttackAction GetInstance() { return new GoblinStealerAttackAction(); }

    Vector3 PlayerPosition;
    CharacterState superArmor;

    Vector3 direction;
    Vector3 destination;
    Vector3 start;
    bool isHit = false;
    

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        NodeUtil.PlayAnim(Owner, "attack");
        PlayerPosition = Player.CurrentPlayer.transform.position ;
        TimelineEvents.Add(new TimeLineEvent(0.6f, TimeLine_4));
        superArmor = new CharacterState(CharacterStateType.E_SuperArmor, Owner);
        Owner.AddState(superArmor);

        direction = (Player.CurrentPlayer.transform.position - Owner.transform.position).normalized;

        start = Owner.transform.position;
        destination = Owner.transform.position + direction * Isometric.IsometricGridSize * 4f;

    }

    public override void UpdateAction()
    {
        base.UpdateAction();
        

        if (!NodeUtil.StateActionMacro(Owner))
        {
            Owner.Status.CurrentSpeed = 20f;

            if(ElapsedTime > 0.2f)
            {
                Owner.NavAgent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
                Owner.NavAgent.Move(Vector3.Lerp(Vector3.zero, destination - Owner.transform.position, 15f * Time.deltaTime));
            }

            if (NodeUtil.IsLastFrame(Owner))
            {
                Owner.Status.CurrentSpeed = Owner.Status.Speed;
                NodeUtil.StopMovement(Owner);
                NodeUtil.ChangeAction(Owner, "GoblinStealerAvoidAction");
                return;
            }
        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
        Owner.NavAgent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        Owner.DeleteState(superArmor);
    }

    void TimeLine_4()
    {
        if(NodeUtil.SweepInRange(start, destination, 2f, Player.CurrentPlayer.transform.position))
        {
            NodeUtil.TakeDamageToPlayer(Owner.Status.CurrentDamage);

            if (NodeUtil.IsActivateAbility(Owner, 215))
            {
                NodeUtil.GiveSilence(Player.CurrentPlayer, 2f);
                NodeUtil.BurnCard();
            }
        }
    }
}
