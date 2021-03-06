﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundPowerAttackAction : PlayerCardAction
{
    public static PlayerGroundPowerAttackAction GetInstance(CardTable dataTable, TargetData target) { return new PlayerGroundPowerAttackAction(dataTable, target); }

    public PlayerGroundPowerAttackAction(CardTable dataTable, TargetData target) : base(dataTable, target)
    {
    }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        AnimUtil.PlayAnim(owner, "ground_power_atk");
        TimelineEvents.Add(new TimeLineEvent(0.22f, SendDamage));
        Owner.AddState(new CharacterState(CharacterStateType.E_SuperArmor, Owner).Init());
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (EntityUtil.StateActionMacro(Owner, CharacterStateType.E_Hold))
        {
            return;
        }

        if (AnimUtil.IsLastFrame(Owner))
        {
            Owner.CurrentAction = PlayerIdleAction.GetInstance();
            return;
        }
    }

    public override void FinishAction()
    {
        base.FinishAction();
        Owner.DeleteState(CharacterStateType.E_SuperArmor);
    }

    public void SendDamage()
    {
        IsoParticle.CreateParticle("BreakGround", Owner.transform.position - Vector3.up * Isometric.IsometricTileSize.y, 0f)
            .RotationAnim("effect_ground_power_atk", Owner.transform.rotation);

        Character[] enemys = Object.FindObjectsOfType<Character>();
        float damage = PlayerUtil.CalculatingCardPowerValue(DataTable._Parameter[0]);

        if (enemys == null)
            return;

        foreach(var e in enemys)
        {
            if (e == Owner) continue;
            float angle = Mathf.Acos(Vector3.Dot((e.transform.position - Owner.transform.position).normalized, Owner.transform.forward))
                * Mathf.Rad2Deg;
            if ((Owner.transform.position - e.transform.position).magnitude <= Isometric.IsometricTileSize.x * 3f &&
                angle < 45f)
            {
                e.AddState(new CharacterHitState(e, damage, 0.1f).Init());
                IsoParticle.CreateParticle("Sliced_Power1", e.transform.position
                    + new Vector3(0f, Isometric.IsometricTileSize.y * 0.5f, 0f),
                    angle);
                IsoParticle.CreateParticle("Sliced_Power2", e.transform.position
                    + new Vector3(0f, Isometric.IsometricTileSize.y * 0.5f, 0f),
                    angle + 90f);
            }
        }

        PlayerUtil.ConsumeCardPowerUpStatus();
    }
}
