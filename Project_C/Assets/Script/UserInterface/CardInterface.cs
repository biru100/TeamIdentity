using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardInterface : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static CardInterface CreateCard()
    {
        return Instantiate(ResourceManager.GetResource<GameObject>("Cards/Card"), InGameInterface.Instance.transform).GetComponent<CardInterface>();
    }

    protected Card _cardData;

    [SerializeField]    protected Image _backSide;
    [SerializeField]    protected Image _frontSide;
    [SerializeField]    protected Text _cardLore;
    [SerializeField]    protected Vector2 _originCardSize;
    int _originFontSize;

    public Image BackSide { get => _backSide; }
    public Image FrontSide { get => _frontSide; }
    public Text CardLore { get => _cardLore; }
    public Vector2 OriginCardSize { get => _originCardSize; }
    public int OriginFontSize { get => _originFontSize; }
    public int HandIndex { get; set; }


    public Card CardData 
    {
        get => _cardData;
        set
        {
            if(_cardData != value)
            {
                _frontSide.sprite = value.FrontSprite;
                _backSide.sprite = value.BackSprite;
                _cardLore.text = value.GetLore(PlayerStatus.CurrentStatus);
                _originFontSize = _cardLore.fontSize;
            }
            _cardData = value;
        }
    }

    public Canvas CardCanvas { get; set; }

    protected CardInterfaceAction _currentAction;

    public CardInterfaceAction CurrentAction
    {
        get => _currentAction;
        set
        {
            _currentAction?.Finish();
            _currentAction = value;
            _currentAction.Start(this);
        }
    }

    private void Awake()
    {
        CardCanvas = GetComponent<Canvas>();
        Material instanceMat = Instantiate(_frontSide.material);
        _frontSide.material = instanceMat;
        _backSide.material = instanceMat;
        _cardLore.material = instanceMat;
    }

    void Start()
    {
        CurrentAction = HandCardAction.GetInstance();
    }

    private void Update()
    {
        _currentAction?.Update();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _currentAction?.OnBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _currentAction?.OnEndDrag(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _currentAction?.OnPointerEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _currentAction?.OnPointerExit(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _currentAction?.OnDrag(eventData);
    }

    public void UpdateLore()
    {
        _cardLore.text = CardData.GetLore(PlayerStatus.CurrentStatus);
    }
}
