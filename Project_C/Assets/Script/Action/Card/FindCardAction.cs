using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FindCardAction : CardInterfaceAction
{
    public static FindCardAction GetInstance() { return ObjectPooling.PopObject<FindCardAction>(); }


    public override void Start(CardInterface owner)
    {
        base.Start(owner);
        owner.Anim.enabled = false;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        CardFindUIInterface.Instance.SelectCard(Owner);
    }
}
