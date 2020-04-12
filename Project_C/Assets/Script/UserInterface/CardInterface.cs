using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardSlotType
{
    E_A, E_S, E_D, E_F, E_Deck
}

[RequireComponent(typeof(Animator))]
public class CardInterface : MonoBehaviour
{
    protected Card _cardData;

    [SerializeField]    protected Image _backSide;
    [SerializeField]    protected Image _frontSide;

    public Animator Anim { get; set; }
    public CardSlotType SlotType { get; set; }
    public Card CardData 
    {
        get => _cardData;
        set
        {
            if(_cardData != value)
            {
                _frontSide.sprite = value.FrontSprite;
                _backSide.sprite = value.BackSprite;
            }
            _cardData = value;
        }
    }

    private void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    public void DrawAction()
    {
        //draw_0,1,2,3
        Anim?.Play("draw_" + (int)SlotType);
    }

    public void ShowAction()
    {
        //show_0,1,2,3
        Anim?.Play("Show_" + (int)SlotType);
    }

    public void UsedAction()
    {
        //use_0,1,2,3
        Anim?.Play("use_" + (int)SlotType);
    }
}
