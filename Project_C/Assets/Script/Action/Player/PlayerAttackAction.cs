using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAction : CharacterAction
{
    public static PlayerAttackAction GetInstance() { return new PlayerAttackAction(); }

    bool isAttackCommand;

    FloatCurve curve;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        AnimUtil.PlayAnim(owner, "attack0");
        isAttackCommand = false;
        TimelineEvents.Add(new TimeLineEvent(0.22f, SendDamage));

        curve = FloatCurve.GetCurve("Curves/AttackMoveCurve");
    }

    public override void UpdateAction()
    {
        base.UpdateAction();
        if (EntityUtil.StateActionMacro(Owner))
        {
            return;
        }


        Owner.NavAgent.Move(Owner.transform.forward * Isometric.IsometricGridSize * Owner.Status.CurrentSpeed * curve.Evaluate(ElapsedTime)  * Time.deltaTime 
            + Owner.transform.forward * Mathf.Clamp01(Vector3.Dot(PlayerUtil.GetVelocityInput(), Owner.transform.forward)) * Isometric.IsometricGridSize * Owner.Status.CurrentSpeed * Time.deltaTime);

        float currentAnimTime = AnimUtil.GetAnimNormalizedTime(Owner);

        if (currentAnimTime > 0.6f)
        {
            if (PlayerUtil.GetAttackInput())
                isAttackCommand = true;
        }

        if (AnimUtil.IsLastFrame(Owner))
        {
            Owner.CurrentAction = isAttackCommand ? (CharacterAction)PlayerAttack1Action.GetInstance() 
                : (CharacterAction)PlayerIdleAction.GetInstance();
            return;

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

        foreach(var e in enemys)
        {
            if (e == Owner) continue;
            float angle = Mathf.Acos(Vector3.Dot((e.transform.position - Owner.transform.position).normalized, Owner.transform.forward))
                * Mathf.Rad2Deg;
            if ((Owner.transform.position - e.transform.position).magnitude <= Isometric.IsometricTileSize.x * 1.8f &&
                angle < 45f)
            {
                e.AddState(new CharacterHitState(e, Owner.Status.CurrentDamage, 0.1f).Init());
                float zAngle  = - 1 * AnimUtil.GetRenderAngle(e.transform.rotation) + 45f + 15f;

                IsoParticle.CreateParticle("Sliced1", e.transform.position
                    + new Vector3(0f, Isometric.IsometricTileSize.y * 0.5f, 0f),
                    zAngle);
                IsoParticle.CreateParticle("Sliced2", e.transform.position
                    + new Vector3(0f, Isometric.IsometricTileSize.y * 0.5f, 0f),
                    zAngle + 90f);
            }
        }
    }
}
