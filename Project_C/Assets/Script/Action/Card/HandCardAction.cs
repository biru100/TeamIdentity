using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBaseAction : CardInterfaceAction
{
    public bool IsHover { get; set; }
    public bool IsDrag { get; set; }
    public bool IsVisible { get; set; }
    public bool IsUsing { get; set; }
    public bool IsUseEffect { get; set; }

    int _originFontSize;
    Vector2 _destCardSize;
    Vector2 _targetPosition;

    Quaternion _destRotation;

    TargetData _target;

    float _elapsedTime;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        InGameInterface.Instance.IsCardDrag = true;
        IsDrag = true;
        _destCardSize = Owner.OriginCardSize;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        Vector3 deltaVector3 = new Vector3(eventData.delta.x, eventData.delta.y);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasHelper.Main.transform as RectTransform, Input.mousePosition, CanvasHelper.Main.worldCamera, out Vector2 mousePosition);
        _targetPosition = mousePosition;

        Quaternion velocityRotation = Quaternion.LookRotation(deltaVector3);

        Vector3 rotationAxis = velocityRotation * Vector3.up;

        _destRotation = Quaternion.AngleAxis(eventData.delta.magnitude * 5f, rotationAxis);

        if (Owner.CardData.TargetType != CardTargetType.E_NonTarget)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(InGameInterface.Instance.HandField, Input.mousePosition))
            {
                if (!IsVisible)
                    ((RectTransform)Owner.transform).anchoredPosition = _targetPosition;
                StopAllCoroutines();
                Owner.FrontSide.material.SetFloat("_DissolveValue", 1f);
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

                if (Owner.CardData.TargetType == CardTargetType.E_Target)
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

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        InGameInterface.Instance.IsCardDrag = false;
        IsDrag = false;

        if (!RectTransformUtility.RectangleContainsScreenPoint(InGameInterface.Instance.HandField, Input.mousePosition))
        {
            if (Owner.CardData.Cost <= PlayerStatus.CurrentStatus.CurrentManaCost && Owner.CardData.TargetType == CardTargetType.E_NonTarget)
            {
                PlayerStatus.CurrentStatus.CurrentManaCost -= Owner.CardData.Cost;
                Player.CurrentPlayer.UseCardStack.Add(new UseCardData(Owner.CardData, _target));
                IsUsing = true;
                InGameInterface.Instance.UseCard(Owner);
                StartCoroutine(DestroyCard());
            }
            //target
            else if (Owner.CardData.TargetType == CardTargetType.E_Target)
            {
                InGameInterface.Instance.ArrowBody.SetActive(false);

                if (Owner.CardData.Cost <= PlayerStatus.CurrentStatus.CurrentManaCost && _target.Target != null)
                {
                    PlayerStatus.CurrentStatus.CurrentManaCost -= Owner.CardData.Cost;
                    Player.CurrentPlayer.UseCardStack.Add(new UseCardData(Owner.CardData, _target));
                    IsUsing = true;
                    InGameInterface.Instance.UseCard(Owner);
                    StartCoroutine(DestroyCard());
                }
                else
                {
                    ((RectTransform)Owner.transform).anchoredPosition = _targetPosition;
                    StopAllCoroutines();
                    Owner.FrontSide.material.SetFloat("_DissolveValue", 1f);
                    IsVisible = true;
                }
            }
            //point
            else if (Owner.CardData.Cost <= PlayerStatus.CurrentStatus.CurrentManaCost)
            {
                PlayerStatus.CurrentStatus.CurrentManaCost -= Owner.CardData.Cost;
                InGameInterface.Instance.ArrowBody.SetActive(false);
                Player.CurrentPlayer.UseCardStack.Add(new UseCardData(Owner.CardData, _target));
                IsUsing = true;
                InGameInterface.Instance.UseCard(Owner);
                StartCoroutine(DestroyCard());
            }
        }
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        IsHover = true;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        IsHover = false;
    }

    void SetUseEffect(bool isEnable)
    {
        if(isEnable != IsUseEffect)
        {
            if(isEnable == true)
            {
                _elapsedTime = 0f;
                IsUseEffect = true;
            }
            else
            {
                IsUseEffect = false;
                Owner.FrontSide.material.SetFloat("_DissolveValue", 1f);
            }
        }
    }

    public override void Start(CardInterface owner)
    {
        base.Start(owner);
    }

    public override void Update()
    {
        base.Update();

        if (!IsUsing)
        {
            if (IsHover && !IsDrag)
            {
                Owner.CardCanvas.sortingOrder = 19;
                _targetPosition = InGameInterface.Instance.GetCardLocationInHand(Owner.HandIndex) + Vector3.up * 100f;
                _destRotation = Quaternion.identity;

                _destCardSize = Owner.OriginCardSize * 1.5f;
                ((RectTransform)Owner.transform).sizeDelta = _destCardSize;
            }
            else if (IsDrag)
            {
                Owner.CardCanvas.sortingOrder = 20;
                _destCardSize = Owner.OriginCardSize;
            }
            else
            {
                Owner.CardCanvas.sortingOrder = Owner.HandIndex + 2;
                _targetPosition = InGameInterface.Instance.GetCardLocationInHand(Owner.HandIndex);
                _destRotation = Quaternion.identity;
                _destCardSize = Owner.OriginCardSize;
            }

            if (IsHover)
                InGameInterface.Instance.MouseOverCard();

            ((RectTransform)Owner.transform).sizeDelta = Vector3.Lerp(((RectTransform)Owner.transform).sizeDelta, _destCardSize,
                3f * Time.unscaledDeltaTime);
            Owner.CardLore.fontSize = (int)((((RectTransform)Owner.transform).sizeDelta.x / Owner.OriginCardSize.x) * _originFontSize);

            if (IsVisible)
            {
                ((RectTransform)Owner.transform).anchoredPosition = Vector2.Lerp(((RectTransform)Owner.transform).anchoredPosition, _targetPosition, 5f * Time.unscaledDeltaTime);
                Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation, _destRotation, 5f * Time.unscaledDeltaTime);
                ((RectTransform)Owner.transform).sizeDelta = Vector3.Lerp(((RectTransform)Owner.transform).sizeDelta, _destCardSize, 3f * Time.unscaledDeltaTime);
            }
        }
    }

    public override void Finish()
    {
        base.Finish();
    }
}
