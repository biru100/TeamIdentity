using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardSlotType
{
    E_A, E_S, E_D, E_F, E_None
}

[RequireComponent(typeof(Animator))]
public class CardInterface : MonoBehaviour
{
    public static CardInterface CreateCard()
    {
        return Instantiate(ResourceManager.GetResource<GameObject>("Cards/Card"), InGameInterface.Instance.transform).GetComponent<CardInterface>();
    }

    protected Card _cardData;

    [SerializeField]    protected Image _backSide;
    [SerializeField]    protected Image _frontSide;
    [SerializeField]    protected Text _cardLore;

    public Animator Anim { get; set; }
    public CardSlotType SlotType { get; set; }
    public bool IsShow { get; set; }

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
        Anim = GetComponent<Animator>();
        Material instanceMat = Instantiate(_frontSide.material);
        _frontSide.material = instanceMat;
        _backSide.material = instanceMat;
        _cardLore.material = instanceMat;
    }

    public void DrawAction()
    {
        //draw_0,1,2,3
        Anim?.Play("draw_" + (int)SlotType);
    }

    public void ShowAction()
    {
        if (!IsShow)
        {
            _cardLore.text = CardData.GetLore();
            Anim?.Play("show_" + (int)SlotType);
            StopAllCoroutines();
            StartCoroutine(ControlTimeScale(0.1f));
            IsShow = true;
        }
    }

    public void HideAction()
    {
        if (IsShow)
        {
            Anim?.Play("hide_" + (int)SlotType);
            StopAllCoroutines();
            StartCoroutine(ControlTimeScale(1f));
            IsShow = false;
        }
    }

    public void UsedAction()
    {
        if (IsShow)
        { 
            StopAllCoroutines();
            StartCoroutine(FadeOutCard());
        }
    }

    IEnumerator ControlTimeScale(float targetTimeScale)
    {
        while(Mathf.Abs(Time.timeScale - targetTimeScale) > 0.005f)
        {
            yield return null;
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, 0.4f);
            Anim.speed = 1f / Time.timeScale;
        }
        Time.timeScale = targetTimeScale;
    }

    IEnumerator FadeOutCard()
    {
        Time.timeScale = 1f;
        Anim.speed = 1f;
        float elapsedTime = 0.5f;
        EntityUtil.ChangeAction(Player.CurrentPlayer, CardData.CardActionName);
        while (elapsedTime > 0f)
        {
            yield return null;
            elapsedTime = Mathf.Max(elapsedTime - Time.deltaTime, 0f);

            _frontSide.material.SetFloat("_DissolveValue", Mathf.Pow(elapsedTime * 2f, 2f));
        }

        Destroy(gameObject);
    }
}
