using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCallTransitionAction : PlayerCardAction
{
    public static PlayerCallTransitionAction GetInstance(CardTable dataTable, TargetData target)
    { return ObjectPooling.PopObject<PlayerCallTransitionAction>().SetData(dataTable, target) as PlayerCallTransitionAction; }


    public override void StartAction(Character owner)
    {
        base.StartAction(owner);
        owner.transform.rotation = Quaternion.Euler(0f, 135f, 0f);
        AnimUtil.PlayAnim(owner, "buff");
        TimelineEvents.Add(new TimeLineEvent(0.1f, AddBuff));
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
    }

    public void AddBuff()
    {
        if (Target.Target != null)
        {
            Vector3 pos = Target.Target.transform.position;
            Target.Target.CurrentAction?.FinishAction();
            GameObject.Destroy(Target.Target.gameObject);
            NodeUtil.CreateEntity("Slime", pos);
        }
    }
}
