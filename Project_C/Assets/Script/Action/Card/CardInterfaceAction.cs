using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInterfaceAction : IPoolObject, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardInterface Owner { get; set; }


    public virtual void OnBeginDrag(PointerEventData eventData)
    {

    }

    public virtual void OnDrag(PointerEventData eventData)
    {

    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {

    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {

    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public virtual void Start(CardInterface owner)
    {
        Owner = owner;
    }

    public virtual void Update()
    {

    }

    public virtual void Finish()
    {

    }

    public virtual void ClearMember()
    {
        Owner = null;
    }
}
