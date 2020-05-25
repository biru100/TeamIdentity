using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    public string DeckName { get; set; }
    public List<Card> DeckCards { get; protected set; }

    public Deck()
    {
        DeckCards = new List<Card>();
    }

    public void AddCard(Card card)
    {
        if (DeckCards.Count >= 30)
            return;

        if (DeckCards.Count == 0)
            DeckCards.Add(card);
        else
        {
            int inputIndex = Random.Range(0, DeckCards.Count);
            Card temp = DeckCards[inputIndex];
            DeckCards[inputIndex] = card;
            DeckCards.Add(temp);
        }
    }

    public void SetCard(List<Card> card)
    {
        Suffle(card);
        DeckCards = card;
    }

    public bool DispenseOneCard(out Card card)
    {
        card = null;
        if (DeckCards.Count != 0)
        {
            card = DeckCards[0];
            DeckCards.RemoveAt(0);
        }

        return card != null;
    }

    void Suffle(List<Card> card)
    {
        for (int i = 0; i < 100; ++i)
        {
            int pre = Random.Range(0, card.Count), next = Random.Range(0, card.Count);
            Card temp = card[pre];
            card[pre] = card[next];
            card[next] = temp;
        }
    }

    public int DeckCount()
    {
        return DeckCards.Count;
    }
}

public class DeckManager : BehaviorSingleton<DeckManager>
{
    public Deck CurrentDeck { get; protected set; }

    public static void LoadDeck(string deckName)
    {
        DeckData selectDeckData = UserData.Instance.OwnedDeckList.Find((data) => data.DeckName == deckName);

        if (Card.CardInstanceSet == null)
            Card.MakeCardInstanceSet(DataManager.GetDatas<CardTable>());

        Deck deck = new Deck();
        foreach (var cardData in selectDeckData.DeckCards)
        {
            for (int i = 0; i < cardData.cardCount; ++i)
            {
                deck.AddCard(Card.CardInstanceSet[DataManager.GetData<CardTable>(cardData.cardIndex)]);
            }
        }

        Instance.CurrentDeck = deck;
    }

    public static void InspectDeckEffectiveness()
    {
        foreach (var deckData in UserData.Instance.OwnedDeckList)
        {
            foreach (var cardData in deckData.DeckCards)
            {
                var ownerCardData = UserData.Instance.OwnedCardList.Find((ucd) => ucd.cardIndex == cardData.cardIndex);
                if (cardData.cardCount > ownerCardData.cardCount)
                {
                    cardData.cardCount = ownerCardData.cardCount;
                    deckData.IsPrepareToUse = false;
                }
            }
        }
    }
}
