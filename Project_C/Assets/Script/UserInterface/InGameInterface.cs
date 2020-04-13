using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameInterface : UIBase<InGameInterface>
{
    protected Dictionary<CardSlotType, CardInterface> SlotCard;

    [SerializeField] protected GameObject _deckImg;

    private void Start()
    {
        SlotCard = new Dictionary<CardSlotType, CardInterface>();
    }
    
    public void SetVisibleDeck(bool isEmpty)
    {
        _deckImg.SetActive(!isEmpty);
    }

    public void DrawCard(Card card, CardSlotType slot)
    {
        CardInterface ci = new CardInterface();
        ci.CardData = card;
        ci.SlotType = slot;
        ci.DrawAction();
        SlotCard[slot] = ci;
    }

    public void UseCard(Card card, CardSlotType slot)
    {
        CardInterface ci = SlotCard[slot];
        ci.UsedAction();
        SlotCard.Remove(slot);
    }

    public void ShowCard(CardSlotType slot)
    {
        SlotCard[slot].ShowAction();
    }
}
