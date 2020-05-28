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
    [SerializeField] protected GameObject _collectCircle;

    [SerializeField] protected Image _hp;
    [SerializeField] protected Text _armor;
    [SerializeField] protected Text _cost;

    [SerializeField] protected Image _slowBackground;


    public RectTransform HandField { get => _handField; set => _handField = value; }
    public GameObject ArrowBody { get => _arrowBody; set => _arrowBody = value; }
    public GameObject CollectCircle { get => _collectCircle; set => _collectCircle = value; }
    public Vector3 DeckPosition { get => _deckImg.transform.localPosition; }
    public bool IsStart { get; set; }
    public bool IsMouseOver { get; set; }
    public bool IsCardDrag { get; set; }

    public int DrawCardQueueCount { get; set; }

    protected override void Awake()
    {
        base.Awake();
        HandCards = new List<CardInterface>();

        StartCoroutine(UpdateCardInterface());
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
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

    public void DrawCard(int DrawCount)
    {
        bool alreadyDrawing = DrawCardQueueCount > 0;
        DrawCardQueueCount += DrawCount;
        if(DrawCardQueueCount > 0 && !alreadyDrawing)
            CreateDrawCard();
    }

    void CreateDrawCard()
    {
        if (DeckManager.Instance.CurrentDeck.DispenseOneCard(out Card dispencedCard))
        {
            CardInterface ci = CardInterface.CreateCard(transform);
            ci.CardData = dispencedCard;

            if (HandCards.Count == 10)
            {
                BurnCardAction burnAction = BurnCardAction.GetInstance();
                burnAction.OnFinish = () => { if (DrawCardQueueCount > 0) CreateDrawCard(); };
                ci.CurrentAction = burnAction;
            }
            else
            {
                HandCards.Add(ci);
                ci.HandIndex = HandCards.Count - 1;
                DrawCardAction drawAction = DrawCardAction.GetInstance();
                drawAction.OnFinish = () => { if (DrawCardQueueCount > 0) CreateDrawCard(); };
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

        return new Vector3((index - half) * 140f, -450f, 0f);
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
