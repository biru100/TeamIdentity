using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameInterface : UIBase<InGameInterface>
{
    protected Dictionary<CardSlotType, CardInterface> SlotCard;

    [SerializeField] protected GameObject _deckImg;

    public static bool IsShowCard { get; set; }
    public static CardSlotType ShowSlot { get; set; }

    protected override void Awake()
    {
        base.Awake();
        SlotCard = new Dictionary<CardSlotType, CardInterface>();
        IsShowCard = false;
        ShowSlot = CardSlotType.E_None;
    }
    
    public void SetVisibleDeck(bool isVisible)
    {
        _deckImg.SetActive(isVisible);
    }

    public void DrawCard(Card card, CardSlotType slot)
    {
        CardInterface ci = CardInterface.CreateCard();
        ci.CardData = card;
        ci.SlotType = slot;
        ci.DrawAction();
        SlotCard[slot] = ci;
    }

    public void ShowCard(CardSlotType slot)
    {
        if (!IsShowCard && SlotCard.ContainsKey(slot))
        {
            ShowSlot = slot;
            IsShowCard = true;
            SlotCard[slot].ShowAction();
        }
    }

    public void HideCard(CardSlotType slot)
    {
        if (IsShowCard && SlotCard.ContainsKey(slot))
        {
            ShowSlot = CardSlotType.E_None;
            IsShowCard = false;
            SlotCard[slot].HideAction();
        }
    }

    public void UseCard(CardSlotType slot)
    {
        if (IsShowCard && ShowSlot == slot)
        {
            CardInterface ci = SlotCard[slot];
            ci.UsedAction();
            SlotCard.Remove(slot);
            IsShowCard = false;
            ShowSlot = CardSlotType.E_None;
        }
    }
}
