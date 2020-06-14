using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardUseAttackAction : PlayerCardAction
{
    public static PlayerCardUseAttackAction GetInstance(CardTable dataTable, TargetData target)
    { return ObjectPooling.PopObject<PlayerCardUseAttackAction>().SetData(dataTable, target) as PlayerCardUseAttackAction; }

    int CardUseCount;
    float damage;

    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        owner.transform.rotation = Quaternion.Euler(0f, 135f, 0f);
        AnimUtil.PlayAnim(owner, "buff");
        CardUseCount = PlayingDataManager.GetModule<UseCardsCountFilterModule>().RoomTotalUseCardCount;
        damage = PlayerUtil.CalculatingCardPowerValue(DataTable._Parameter[0]);
        PlayerUtil.ConsumeCardPowerUpStatus();
        for (int i = 0; i < CardUseCount; ++i)
        {
            TimelineEvents.Add(new TimeLineEvent(0.1f * (i + 1), CallAttack));
        }


    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (EntityUtil.StateActionMacro(Owner, CharacterStateType.E_Hold))
        {
            return;
        }

        if (AnimUtil.IsLastFrame(Owner) || Target.Target.Status.CurrentHp <= 0)
        {
            Owner.CurrentAction = PlayerIdleAction.GetInstance();
            return;

        }


    }

    public override void FinishAction()
    {
        base.FinishAction();
    }

    public void CallAttack()
    {

        Target.Target.AddState(new CharacterHitState(Target.Target, damage, 0.1f).Init());
    }
}
