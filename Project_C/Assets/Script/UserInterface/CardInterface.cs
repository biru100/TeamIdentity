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

    public int HandIndex { get; set; }
    public bool IsHover { get; set; }
    public bool IsDrag { get; set; }
    public bool IsVisible { get; set; }
    public bool IsUsing { get; set; }

    int _originFontSize;
    Vector2 _destCardSize;
    Vector2 _targetPosition;

    Quaternion _destRotation;

    TargetData _target;

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
                _originFontSize = _cardLore.fontSize;
            }
            _cardData = value;
        }
    }

    public Canvas CardCanvas { get; set; }

    private void Awake()
    {
        CardCanvas = GetComponent<Canvas>();
        Material instanceMat = Instantiate(_frontSide.material);
        _frontSide.material = instanceMat;
        _backSide.material = instanceMat;
        _cardLore.material = instanceMat;

        _destCardSize = _originCardSize;
        _destRotation = Quaternion.identity;

        _target = new TargetData();

        IsVisible = true;
    }

    private void Update()
    {
        if (!IsUsing)
        {
            if (IsHover && !IsDrag)
            {
                CardCanvas.sortingOrder = 19;
                _targetPosition = InGameInterface.Instance.GetCardLocationInHand(HandIndex) + Vector3.up * 100f;
                _destRotation = Quaternion.identity;

                _destCardSize = _originCardSize * 1.5f;
                ((RectTransform)transform).sizeDelta = _destCardSize;
            }
            else if (IsDrag)
            {
                CardCanvas.sortingOrder = 20;
                _destCardSize = _originCardSize;
            }
            else
            {
                CardCanvas.sortingOrder = HandIndex + 2;
                _targetPosition = InGameInterface.Instance.GetCardLocationInHand(HandIndex);
                _destRotation = Quaternion.identity;
                _destCardSize = _originCardSize;
            }

            if (IsHover)
                InGameInterface.Instance.MouseOverCard();

            ((RectTransform)transform).sizeDelta = Vector3.Lerp(((RectTransform)transform).sizeDelta, _destCardSize,
                3f * Time.unscaledDeltaTime);
            _cardLore.fontSize = (int)((((RectTransform)transform).sizeDelta.x / _originCardSize.x) * _originFontSize);

            if (IsVisible)
            {
                ((RectTransform)transform).anchoredPosition = Vector2.Lerp(((RectTransform)transform).anchoredPosition, _targetPosition, 5f * Time.unscaledDeltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, _destRotation, 5f * Time.unscaledDeltaTime);
                ((RectTransform)transform).sizeDelta = Vector3.Lerp(((RectTransform)transform).sizeDelta, _destCardSize, 3f * Time.unscaledDeltaTime);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InGameInterface.Instance.IsCardDrag = true;
        IsDrag = true;
        _destCardSize = _originCardSize;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        InGameInterface.Instance.IsCardDrag = false;
        IsDrag = false;

        if(!RectTransformUtility.RectangleContainsScreenPoint(InGameInterface.Instance.HandField, Input.mousePosition))
        {
            if (CardData.TargetType == CardTargetType.E_NonTarget)
            {
                Player.CurrentPlayer.UseCardStack.Add(new UseCardData(CardData, _target));
                StartCoroutine(DestroyCard());
                IsUsing = true;
                DisableEventSystem();
                InGameInterface.Instance.UseCard(this);
            }
            //target
            else if(CardData.TargetType == CardTargetType.E_Target)
            {
                InGameInterface.Instance.ArrowBody.SetActive(false);

                if (_target.Target != null)
                {
                    Player.CurrentPlayer.UseCardStack.Add(new UseCardData(CardData, _target));
                    StartCoroutine(DestroyCard());
                    IsUsing = true;
                    DisableEventSystem();
                    InGameInterface.Instance.UseCard(this);
                }
                else
                {
                    ((RectTransform)transform).anchoredPosition = _targetPosition;
                    StopAllCoroutines();
                    _frontSide.material.SetFloat("_DissolveValue", 1f);
                    IsVisible = true;
                }
            }
            //point
            else
            {
                InGameInterface.Instance.ArrowBody.SetActive(false);
                Player.CurrentPlayer.UseCardStack.Add(new UseCardData(CardData, _target));
                StartCoroutine(DestroyCard());
                IsUsing = true;
                DisableEventSystem();
                InGameInterface.Instance.UseCard(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsHover = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 deltaVector3 = new Vector3(eventData.delta.x, eventData.delta.y);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasHelper.Main.transform as RectTransform, Input.mousePosition, CanvasHelper.Main.worldCamera, out Vector2 mousePosition);
        _targetPosition = mousePosition;

        Quaternion velocityRotation = Quaternion.LookRotation(deltaVector3);

        Vector3 rotationAxis = velocityRotation * Vector3.up;

        _destRotation = Quaternion.AngleAxis(eventData.delta.magnitude * 5f, rotationAxis);

        if (CardData.TargetType != CardTargetType.E_NonTarget)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(InGameInterface.Instance.HandField, Input.mousePosition))
            {
                if(!IsVisible)
                    ((RectTransform)transform).anchoredPosition = _targetPosition;
                StopAllCoroutines();
                _frontSide.material.SetFloat("_DissolveValue", 1f);
                IsVisible = true;

                InGameInterface.Instance.ArrowBody.SetActive(false);
            }
            else
            {
                InGameInterface.Instance.ArrowBody.SetActive(true);

                if (IsVisible)
                {
                    StartCoroutine(DestroyCardEffect());
                }

                Vector3 basePos = InGameInterface.Instance.GetCardLocationInHand(HandIndex);
                Vector3 target = new Vector3(_targetPosition.x, _targetPosition.y, 0f);
                InGameInterface.Instance.ArrowBody.transform.localPosition = basePos;

                Vector2 arrowSize = ((RectTransform)InGameInterface.Instance.ArrowBody.transform).sizeDelta;
                arrowSize.y = (target - basePos).magnitude - 100f;
                ((RectTransform)InGameInterface.Instance.ArrowBody.transform).sizeDelta = arrowSize;

                Quaternion arrowRotation = Quaternion.FromToRotation(Vector3.up, (target - basePos).normalized);
                InGameInterface.Instance.ArrowBody.transform.localRotation = arrowRotation;

                if (CardData.TargetType == CardTargetType.E_Target)
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    if (hit.collider != null)
                    {
                        _target.SetTarget(hit.collider.GetComponentInParent<Character>());

                        if (_target.Target is Player)
                            _target.SetTarget(null);
                    }
                    else
                    {
                        _target.SetTarget(null);
                    }

                    InGameInterface.Instance.CollectCircle.SetActive(_target.Target != null);
                }
                else
                {
                    //Isometric.GetIsometicBasePositionByWorldRay(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                    //    Camera.main.transform.forward);
                }
            }
        }
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

    IEnumerator DestroyCardEffect()
    {
        IsVisible = false;
        float elapsedTime = 0f;

        while(elapsedTime < 0.5f)
        {
            yield return null;
            elapsedTime += Time.unscaledDeltaTime;
            _frontSide.material.SetFloat("_DissolveValue", 1f - elapsedTime * 2f);
        }
    }

    IEnumerator DestroyCard()
    {
        yield return StartCoroutine(DestroyCardEffect());
        Destroy(gameObject);
    }
}
