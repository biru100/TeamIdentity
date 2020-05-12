using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEverybodySlimeAction : PlayerCardAction
{
    public static PlayerEverybodySlimeAction GetInstance(CardTable dataTable, TargetData target) { return new PlayerEverybodySlimeAction(dataTable, target); }

    public PlayerEverybodySlimeAction(CardTable dataTable, TargetData target) : base(dataTable, target)
    {
    }

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

    public void AddBuff()
    {

        Character[] enemys = Object.FindObjectsOfType<Character>();
        
        if (enemys == null)
            return;

        foreach(var e in enemys)
        {
            if (e == Owner) continue;
            float angle = Mathf.Acos(Vector3.Dot((e.transform.position - Owner.transform.position).normalized, Owner.transform.forward))
                * Mathf.Rad2Deg;
            if ((Owner.transform.position - e.transform.position).magnitude <= Isometric.IsometricTileSize.x * 4f)
            {
                Vector3 pos = e.transform.position;
                Target.Target.CurrentAction?.FinishAction();
                GameObject.Destroy(e.gameObject);
                NodeUtil.CreateEntity("Slime", pos);
            }
        }

        PlayerUtil.ConsumeCardPowerUpStatus();
        }
}

