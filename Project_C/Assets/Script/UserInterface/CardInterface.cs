using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum CardSlotType
{
    E_A, E_S, E_D, E_F, E_None
}

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

    public CardSlotType SlotType { get; set; }
    public int HandIndex { get; set; }
    public bool IsHover { get; set; }
    public bool IsDrag { get; set; }
    public bool IsUsing { get; set; }

    Vector2 _destCardSize;
    Vector2 _targetPosition;

    Quaternion _destRotation;

    public Card CardData 
    {
        get => _cardData;
        set
        {
            if(_cardData != value)
            {
                _frontSide.sprite = value.FrontSprite;
                _backSide.sprite = value.BackSprite;
                _cardLore.text = value.GetLore();
            }
            _cardData = value;
        }
    }

    private void Awake()
    {
        Material instanceMat = Instantiate(_frontSide.material);
        _frontSide.material = instanceMat;
        _backSide.material = instanceMat;
        _cardLore.material = instanceMat;

        _destCardSize = _originCardSize;
        _destRotation = Quaternion.identity;
    }

    private void Update()
    {
        if (!IsUsing)
        {
            if(IsHover)
                InGameInterface.Instance.MouseOverCard();

            ((RectTransform)transform).sizeDelta = Vector3.Lerp(((RectTransform)transform).sizeDelta, _destCardSize,
                3f * Time.unscaledDeltaTime);

            if (!IsDrag)
            {
                _targetPosition = InGameInterface.Instance.GetCardLocationInHand(HandIndex);
                _destRotation = Quaternion.identity;
            }

        ((RectTransform)transform).anchoredPosition = Vector2.Lerp(((RectTransform)transform).anchoredPosition, _targetPosition, 5f * Time.unscaledDeltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, _destRotation, 5f * Time.unscaledDeltaTime);
            ((RectTransform)transform).sizeDelta = Vector3.Lerp(((RectTransform)transform).sizeDelta, _destCardSize, 3f * Time.unscaledDeltaTime);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InGameInterface.Instance.IsCardDrag = true;
        IsDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        InGameInterface.Instance.IsCardDrag = false;
        IsDrag = false;

        if(!RectTransformUtility.RectangleContainsScreenPoint(InGameInterface.Instance.HandField, Input.mousePosition))
        {
            NodeUtil.ChangeAction(Player.CurrentPlayer, CardData.CardActionName);
            StartCoroutine(DestroyCard());
            IsUsing = true;
            DisableEventSystem();
            InGameInterface.Instance.UseCard(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsHover = true;

        if (!IsDrag)
            _destCardSize = _originCardSize * 1.5f;
        else
            _destCardSize = _originCardSize;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsHover = false;
        _destCardSize = _originCardSize;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 deltaVector3 = new Vector3(eventData.delta.x, eventData.delta.y);

        _targetPosition += eventData.delta * 1f / transform.lossyScale ;

        Quaternion velocityRotation = Quaternion.LookRotation(deltaVector3);

        Vector3 rotationAxis = velocityRotation * Vector3.up;

        _destRotation = Quaternion.AngleAxis(eventData.delta.magnitude * 5f, rotationAxis);
    }

    public void DisableEventSystem()
    {
        _frontSide.raycastTarget = false;
        _backSide.raycastTarget = false;
        _cardLore.raycastTarget = false;
    }

    public void EnableEventSystem()
    {
        _frontSide.raycastTarget = true;
        _backSide.raycastTarget = true;
        _cardLore.raycastTarget = true;
    }

    public void UpdateLore()
    {
        _cardLore.text = CardData.GetLore();
    }

    IEnumerator DestroyCard()
    {
        float elapsedTime = 0f;

        while(elapsedTime < 0.5f)
        {
            yield return null;
            elapsedTime += Time.unscaledDeltaTime;
            _frontSide.material.SetFloat("_DissolveValue", 1f - elapsedTime * 2f);
        }

        Destroy(gameObject);
    }
}
