using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack1Action : CharacterAction
{
    public static PlayerAttack1Action GetInstance() { return ObjectPooling.PopObject<PlayerAttack1Action>(); }

    bool isAttackCommand;
    MovementSetController movementAnimController;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        AnimUtil.PlayAnim(owner, "attack1");
        isAttackCommand = false;
        TimelineEvents.Add(new TimeLineEvent(0.22f, SendDamage));

        movementAnimController = MovementSetController.GetMovementSetByAngle(owner.transform.rotation, "MovementList/Attack2Movement");
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (EntityUtil.StateActionMacro(Owner))
        {
            return;
        }

        float currentAnimTime = AnimUtil.GetAnimNormalizedTime(Owner);

        Vector3 offset;
        if (movementAnimController.GetMovementData(Time.deltaTime, out offset))
        {
            Owner.NavAgent.Move(offset);
        }

        if (AnimUtil.IsLastFrame(Owner))
        {
            Owner.CurrentAction = (CharacterAction)PlayerIdleAction.GetInstance();
            return;

        }

        if (currentAnimTime >= 0.68f && isAttackCommand)
        {
            Owner.CurrentAction = (CharacterAction)PlayerAttack2Action.GetInstance();
            return;
        }

        if (currentAnimTime < 0.67f)
        {
            if (PlayerUtil.GetAttackInput())
                isAttackCommand = true;
        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
    }

    public void SendDamage()
    {

        Character[] enemys = Object.FindObjectsOfType<Character>();

        if (enemys == null)
            return;

        foreach (var e in enemys)
        {
            if (e == Owner) continue;
            float angle = Mathf.Acos(Vector3.Dot((e.transform.position - Owner.transform.position).normalized, Owner.transform.forward))
                * Mathf.Rad2Deg;
            if ((Owner.transform.position - e.transform.position).magnitude <= Isometric.IsometricTileSize.x * 2.5f &&
                angle < 30f)
            {
                e.AddState(new CharacterHitState(e, Owner.Status.CurrentDamage, 0.1f).Init());
                IsoParticle.CreateParticle("Sliced1", e.transform.position
                    + new Vector3(0f, Isometric.IsometricTileSize.y * 0.5f, 0f),
                    angle);
                IsoParticle.CreateParticle("Sliced2", e.transform.position
                    + new Vector3(0f, Isometric.IsometricTileSize.y * 0.5f, 0f),
                    angle + 90f);
            }
        }
    }
}
