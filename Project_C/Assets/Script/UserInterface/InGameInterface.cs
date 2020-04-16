using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck
{

    protected static Deck _instance;

    public static Deck Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Deck();
            return _instance;
        }
    }

    protected List<Card> DeckCards;

    protected Deck()
    {
        DeckCards = new List<Card>();
    }

    public void AddCard(Card card)
    {
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
        for(int i = 0; i < 100; ++i)
        {
            int pre = Random.Range(0, card.Count), next = Random.Range(0, card.Count);
            Card temp = card[pre];
            card[pre] = card[next];
            card[next] = temp;
        }
    }

    public void Destroy()
    {
        _instance = null;
    }
}

public class InGameInterface : UIBase<InGameInterface>
{
    protected List<CardInterface> HandCards;

    [SerializeField] protected GameObject _deckImg;

    [SerializeField] protected RectTransform _handField;

    public RectTransform HandField { get => _handField; set => _handField = value; }
    public bool IsStart { get; set; }
    public bool IsMouseOver { get; set; }
    public bool IsCardDrag { get; set; }

    protected override void Awake()
    {
        base.Awake();
        HandCards = new List<CardInterface>();
    }

    private void Update()
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, IsMouseOver || IsCardDrag ? 0.05f : 1f, 0.05f);
        IsMouseOver = false;
    }

    public void DrawCard()
    {
        if (Deck.Instance.DispenseOneCard(out Card dispencedCard))
        {
            CardInterface ci = CardInterface.CreateCard();
            ci.CardData = dispencedCard;
            
            if(HandCards.Count == 10)
            {
                //draw burnCard ani
                Destroy(ci.gameObject);
            }
            else
            {
                HandCards.Add(ci);
                ci.HandIndex = HandCards.Count - 1;
                //draw card ani
            }
        }
    }

    public void UseCard(CardInterface card)
    {
        HandCards.Remove(card);
        for(int i = 0; i < HandCards.Count; ++i)
        {
            HandCards[i].UpdateLore();
            HandCards[i].HandIndex = i;
        }
    }

    public Vector3 GetCardLocationInHand(int index)
    {
        float half = (HandCards.Count - 1) * 0.5f;

        return new Vector3((index - half) * 250f, -380f, 0f);
    }
    
    public void MouseOverCard()
    {
        IsMouseOver = true;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Deck.Instance.Destroy();
    }
}
