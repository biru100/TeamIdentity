using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStatus : CharacterStatus
{
    public static PlayerStatus CurrentStatus{ get; set; }

    protected List<Card> Deck { get; set; }
    public Dictionary<CardSlotType, Card> SlotCard { get; set; }

    public PlayerStatus(Character owner) : base(owner)
    {
        CurrentStatus = this;
        Deck = new List<Card>();
        SlotCard = new Dictionary<CardSlotType, Card>();
    }

    public void AddCard(Card card, bool isDirectDraw = false)
    {
        Deck.Add(card);
        InGameInterface.Instance.SetVisibleDeck(true);

        if(isDirectDraw && SlotCard.Count < 4)
        {
            for(int i = 0; i < 4; ++i)
            {
                if (!SlotCard.ContainsKey((CardSlotType)i))
                {
                    DrawCard((CardSlotType)i);
                    break;
                }
            }
        }
    }

    public void DrawCard(CardSlotType newSlot)
    {
        if (Deck.Count == 0)
            return;
        SlotCard[newSlot] = Deck[0];
        Deck.RemoveAt(0);
        InGameInterface.Instance.DrawCard(SlotCard[newSlot], newSlot);
        InGameInterface.Instance.SetVisibleDeck(false);
    }

    public bool UseCard(CardSlotType newSlot)
    {
        if (!SlotCard.ContainsKey(newSlot))
            return false;

        Card useCard = SlotCard[newSlot];
        SlotCard.Remove(newSlot);
        InGameInterface.Instance.UseCard(useCard, newSlot);
        DrawCard(newSlot);
        return true;
    }

    public bool ShowCard(CardSlotType newSlot)
    {
        if (!SlotCard.ContainsKey(newSlot))
            return false;

        InGameInterface.Instance.ShowCard(newSlot);
        return true;
    }
}

public class CharacterStatus
{
    public Character Owner { get; set; }

    public float Damage { get; set; }
    public float CurrentDamage { get; set; }
    public float Armor { get; set; }
    public float CurrentArmor { get; set; }
    public float Hp { get; set; }
    public float CurrentHp { get; set; }
    public float Speed { get; set; }
    public float CurrentSpeed { get; set; }

    public CharacterStatus(Character owner)
    {
        Owner = owner;

        Damage = 10f;
        CurrentDamage = 10f;

        Armor = 0f;
        CurrentArmor = 0f;

        Hp = 100f;
        CurrentHp = 100f;


        Speed = 1.2f;
        CurrentSpeed = 1.2f;
    }

    public bool UpdateStatus(CharacterNotifyEvent notify)
    {
        if(notify.Type == CharacterNotifyType.E_Damage && CurrentHp > 0f)
        {
            CurrentHp = Mathf.Max(CurrentHp - (float)notify.Data, 0f);
            if(CurrentHp <= 0f)
            {
                Owner.ConsumeNotifyEvent(notify);
                Owner.AddNotifyEvent(new CharacterNotifyEvent(CharacterNotifyType.E_Dead, null));
                return false;
            }
        }

        return true;
    }
}
