using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEverybodySlimeAction : PlayerCardAction
{
    public static PlayerEverybodySlimeAction GetInstance(CardTable dataTable, TargetData target)
    { return ObjectPooling.PopObject<PlayerEverybodySlimeAction>().SetData(dataTable, target) as PlayerEverybodySlimeAction; }


    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        AnimUtil.PlayAnim(owner, "buff");
        TimelineEvents.Add(new TimeLineEvent(0.22f, AddBuff));
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

    public void AddBuff()
    {

        Character[] enemys = Object.FindObjectsOfType<Character>();

        if (enemys == null)
            return;

        foreach (var e in enemys)
        {
            if (e == Owner) continue;

            if ((Owner.transform.position - e.transform.position).magnitude <= Isometric.IsometricTileSize.x * 3f)
            {
                if (e != null)
                {
                    Vector3 pos = e.transform.position;
                    e.CurrentAction?.FinishAction();
                    GameObject.Destroy(e.gameObject);
                    NodeUtil.CreateEntity("Slime", pos);
                }
                IsoParticle.CreateParticle("Sliced_Power1", e.transform.position
                    + new Vector3(0f, Isometric.IsometricTileSize.y * 0.5f, 0f), 0f);
            }
        }

        PlayerUtil.ConsumeCardPowerUpStatus();
    }

    public void Trans()
    {

    }
}

