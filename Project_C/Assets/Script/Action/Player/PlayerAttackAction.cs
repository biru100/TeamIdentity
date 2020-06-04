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
        TimelineEvents.Add(new TimeLineEvent(0.1f, SendDamage));

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

        Monster[] enemys = Object.FindObjectsOfType<Monster>();

        if (enemys == null || enemys.Length == 0)
            return;

        bool isValidAttack = false;

        foreach (var e in enemys)
        {
            if (e == Owner) continue;

            Vector3 direction = e.RenderTrasform.GetIsometricPosition() - Owner.RenderTrasform.GetIsometricPosition();
            direction.z = 0f;

            float angle = Quaternion.FromToRotation(Vector3.right, direction.normalized).eulerAngles.z;
            if ((Owner.transform.position - e.transform.position).magnitude <= Isometric.IsometricTileSize.x * 2f &&
                Mathf.Acos(Vector3.Dot((e.transform.position - Owner.transform.position).normalized, Owner.transform.forward)) * Mathf.Rad2Deg < 65f)
            {
                isValidAttack = true;
                e.AddState(new CharacterHitState(e, Owner.Status.CurrentDamage, 0.1f).Init());

                angle += 12f;

                IsoParticle.CreateParticle("Sliced1", e.transform.position
                    + Vector3.up * Isometric.IsometricTileSize.y * e.EffectOffset,
                    angle);
                IsoParticle.CreateParticle("Sliced2", e.transform.position
                    + Vector3.up * Isometric.IsometricTileSize.y * e.EffectOffset,
                    Random.Range(0, 360));
            }
        }

        if(isValidAttack)
            CameraManager.PlayAnim("attack", AnimUtil.GetRenderAngle(Owner.transform.rotation));
    }
}
