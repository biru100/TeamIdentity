using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameInterface : ManagedUIInterface<InGameInterface>
{
    protected List<CardInterface> HandCards;

    [SerializeField] protected GameObject _deckImg;
    [SerializeField] protected Text _deckCount;

    [SerializeField] protected RectTransform _handField;
    [SerializeField] protected GameObject _arrowBody;

    [SerializeField] protected Image _hp;
    [SerializeField] protected Text _armor;
    [SerializeField] protected Text _cost;

    [SerializeField] protected Image _slowBackground;

    public CardState GlobalCardState { get; protected set; }


    public RectTransform HandField { get => _handField; set => _handField = value; }
    public GameObject ArrowBody { get => _arrowBody; set => _arrowBody = value; }
    public Vector3 DeckPosition { get => _deckImg.transform.localPosition; }
    public bool IsStart { get; set; }
    public bool IsMouseOver { get; set; }
    public bool IsCardDrag { get; set; }

    public int CurrentHandCardCount { get => HandCards?.Count ?? 0; }
    public int MaxHandCardCount { get => 10; }

    public int DrawCardQueueCount { get; set; }
    public bool IsDrawing { get; set; }

    protected override void Awake()
    {
        base.Awake();
        HandCards = new List<CardInterface>();
        GlobalCardState = new CardState();

        StartCoroutine(UpdateCardInterface());
    }

    private void Update()
    {
        if(IsCardDrag || Input.GetKey(KeyCode.Space))
        {
            _slowBackground.raycastTarget = true;
            _slowBackground.color = Color.Lerp(_slowBackground.color, new Color(0f, 0f, 0f, 0.3f), 3f * Time.unscaledDeltaTime);
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.05f, 3f * Time.unscaledDeltaTime);
        }
        else
        {
            _slowBackground.raycastTarget = false;
            _slowBackground.color = Color.Lerp(_slowBackground.color, new Color(0f, 0f, 0f, 0f), 3f * Time.unscaledDeltaTime);
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 3f * Time.unscaledDeltaTime);
        }

        if(DrawCardQueueCount > 0 && !IsDrawing)
        {
            CreateDrawCard();
        }

        IsMouseOver = false;
        _deckCount.text = DeckManager.Instance.CurrentDeck.DeckCount().ToString();
        _deckImg.SetActive(DeckManager.Instance.CurrentDeck.DeckCount() != 0);
        if(PlayerStatus.CurrentStatus != null)
        {
            _hp.fillAmount = PlayerStatus.CurrentStatus.CurrentHp / PlayerStatus.CurrentStatus.Hp;
            _armor.text = PlayerStatus.CurrentStatus.CurrentArmor.ToString();
            _cost.text = PlayerStatus.CurrentStatus.CurrentManaCost.ToString();
        }
    }

    public void AddToHand(CardInterface ci)
    {
        if (HandCards.Count < MaxHandCardCount)
        {
            HandCards.Add(ci);
            ci.transform.parent = transform;
            ci.HandIndex = HandCards.Count - 1;
            ci.CurrentAction = HandCardAction.GetInstance();
        }
    }

    public void DrawCard(int DrawCount)
    {
        DrawCardQueueCount += DrawCount;
    }

    void CreateDrawCard()
    {
        IsDrawing = true;

        if (DeckManager.Instance.CurrentDeck.DispenseOneCard(out Card dispencedCard))
        {
            CardInterface ci = CardInterface.CreateCard(transform);
            ci.CardData = dispencedCard;

            if (HandCards.Count == MaxHandCardCount)
            {
                BurnCardAction burnAction = BurnCardAction.GetInstance();
                burnAction.OnFinish = () => { IsDrawing = false; };
                ci.CurrentAction = burnAction;
            }
            else
            {
                HandCards.Add(ci);
                ci.HandIndex = HandCards.Count - 1;
                DrawCardAction drawAction = DrawCardAction.GetInstance();
                drawAction.OnFinish = () => { IsDrawing = false; };
                ci.CurrentAction = drawAction;
            }
        }
        DrawCardQueueCount = Mathf.Max(DrawCardQueueCount - 1, 0);
    }

    public void DestroyCard()
    {
        if (DeckManager.Instance.CurrentDeck.DispenseOneCard(out Card dispencedCard))
        {
            CardInterface ci = CardInterface.CreateCard(transform);
            ci.CardData = dispencedCard;
            ci.CurrentAction = BurnCardAction.GetInstance();
        }
    }

    IEnumerator UpdateCardInterface()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < HandCards.Count; ++i)
            {
                HandCards[i].UpdateLore();
                HandCards[i].HandIndex = i;
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

        return Quaternion.Euler(0f, 0f, -(index - half) * 5f) * new Vector3(0f, 2000f, 0f) + new Vector3(0f, -2410f, 0f);
    }

    public Quaternion GetCardRotationInHand(int index)
    {
        float half = (HandCards.Count - 1) * 0.5f;

        return Quaternion.Euler(0f, 0f, -(index - half) * 5f);
    }

    public void MouseOverCard()
    {
        //IsMouseOver = true;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
