using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAction : CharacterAction
{
    public static PlayerAttackAction GetInstance() { return ObjectPooling.PopObject<PlayerAttackAction>(); }

    bool isAttackCommand;
    MovementSetController movementAnimController;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        isAttackCommand = false;
        TimelineEvents.Add(new TimeLineEvent(0.22f, SendDamage));

        owner.transform.rotation = EffectiveUtility.GetMouseRotation(owner.transform);
        AnimUtil.PlayAnim(owner, "attack0");

        movementAnimController = MovementSetController.GetMovementSetByAngle(owner.transform.rotation, "MovementList/Attack1Movement");
    }

    public override void UpdateAction()
    {
        base.UpdateAction();
        if (EntityUtil.StateActionMacro(Owner))
        {
            return;
        }

        //Owner.NavAgent.Move(Owner.transform.forward * Isometric.IsometricGridSize * Owner.Status.CurrentSpeed * 
        //    curve.Evaluate(ElapsedTime) * (Mathf.Clamp01(Vector3.Dot(PlayerUtil.GetVelocityInput(), Owner.transform.forward)) + 1f) 
        //    * Time.deltaTime);

        Vector3 offset;
        if(movementAnimController.GetMovementData(Time.deltaTime, out offset))
        {
            Owner.NavAgent.Move(offset);
        }

        float currentAnimTime = AnimUtil.GetAnimNormalizedTime(Owner);

        if (AnimUtil.IsLastFrame(Owner))
        {
            Owner.CurrentAction = isAttackCommand ? (CharacterAction)PlayerAttack1Action.GetInstance()
                : (CharacterAction)PlayerIdleAction.GetInstance();
            return;

        }

        if (currentAnimTime >= 0.78f && isAttackCommand)
        {
            Owner.CurrentAction = PlayerAttack1Action.GetInstance();
            return;
        }

        if (currentAnimTime <= 0.77f)
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

        Character[] enemys = Object.FindObjectsOfType<Monster>();

        if (enemys == null || enemys.Length == 0)
            return;

        CameraManager.PlayAnim("attack", 0);

        foreach (var e in enemys)
        {
            if (e == Owner) continue;
            float angle = Mathf.Acos(Vector3.Dot((e.transform.position - Owner.transform.position).normalized, Owner.transform.forward))
                * Mathf.Rad2Deg;
            if ((Owner.transform.position - e.transform.position).magnitude <= Isometric.IsometricTileSize.x * 1.8f &&
                angle < 45f)
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
