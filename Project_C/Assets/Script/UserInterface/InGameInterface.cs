using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameInterface : UIBase<InGameInterface>
{
    protected List<CardInterface> Deck;

    protected Dictionary<CardSlotType, CardInterface> SlotCard;

    private void Start()
    {
        Deck = new List<CardInterface>();
        SlotCard = new Dictionary<CardSlotType, CardInterface>();
    }

}
