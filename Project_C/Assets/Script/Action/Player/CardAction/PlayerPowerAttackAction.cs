using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerAttackAction : PlayerCardAction
{
    public static PlayerPowerAttackAction GetInstance(CardTable dataTable, TargetData target)
    { return ObjectPooling.PopObject<PlayerPowerAttackAction>().SetData(dataTable, target) as PlayerPowerAttackAction; }

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);

        owner.transform.rotation = EffectiveUtility.GetMouseRotation(owner.transform);

        AnimUtil.PlayAnim(owner, "power_atk");
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

        Owner.NavAgent.Move(Owner.transform.forward * Isometric.IsometricTileSize.x * 0.5f * Time.deltaTime);

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

        Character[] enemys = Object.FindObjectsOfType<Character>();
        float damage = PlayerUtil.CalculatingCardPowerValue(DataTable._Parameter[0]);

        if (enemys == null)
            return;

        foreach(var e in enemys)
        {
            if (e == Owner) continue;
            float angle = Mathf.Acos(Vector3.Dot((e.transform.position - Owner.transform.position).normalized, Owner.transform.forward))
                * Mathf.Rad2Deg;
            if ((Owner.transform.position - e.transform.position).magnitude <= Isometric.IsometricTileSize.x * 1.8f &&
                angle < 80f)
            {
                e.AddState(new CharacterHitState(e, damage, 0.1f).Init());
                IsoParticle.CreateParticle("Sliced_Power1", e.transform.position
                    + Vector3.up * Isometric.IsometricTileSize.y * e.EffectOffset,
                    angle);
                IsoParticle.CreateParticle("Sliced_Power2", e.transform.position
                    + Vector3.up * Isometric.IsometricTileSize.y * e.EffectOffset,
                    angle + 90f);
            }
        }

        PlayerUtil.ConsumeCardPowerUpStatus();
    }
}
