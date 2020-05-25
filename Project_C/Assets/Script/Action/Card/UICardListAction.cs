using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardListElementDisplayType
{  
    E_Idle,
    E_NonOwned,
    E_NonMoreEquited
}

public class UICardListAction : CardInterfaceAction
{
    public static UICardListAction GetInstance() { return new UICardListAction(); }

    CardListElementDisplayType currentDisplayType = CardListElementDisplayType.E_Idle;

    public int CardCount { get; set; }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (currentDisplayType == CardListElementDisplayType.E_Idle)
                DeckBuildingUIInterface.Instance.DeckView.AddCard(Owner.CardData.Data);

            SettingUIDisplayMode();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            //view
        }
    }

    public override void Start(CardInterface owner)
    {
        base.Start(owner);
        owner.Anim.enabled = false;
        Owner.CardCanvas.sortingOrder = 1;

        (Owner.transform as RectTransform).sizeDelta = Owner.OriginCardSize * 1.5f;
        Owner.CardCanvas.overrideSorting = false;

        SettingUIDisplayMode();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Finish()
    {
        base.Finish();
        Owner.CardCanvas.overrideSorting = true;
        Owner.GrayScaleValue = 0f;
        Owner.AlphaValue = 1f;
        (Owner.transform as RectTransform).anchorMin = Vector2.one * 0.5f;
        (Owner.transform as RectTransform).anchorMax = Vector2.one * 0.5f;
    }

    void SettingUIDisplayMode()
    {
        UserCardData userCard = UserData.Instance.OwnedCardList.Find((usd) => usd.cardIndex == Owner.CardData.Data._Index);
        UserCardData deckCard = DeckBuildingUIInterface.Instance.DeckView.ControlledDeckData.DeckCards.Find((card) => card.cardIndex == Owner.CardData.Data._Index);
        int alreadyInDeckCount = deckCard?.cardCount ?? 0;

        Debug.Log(alreadyInDeckCount);

        if (userCard != null)
        {
            CardCount = userCard.cardCount;
        }

        //non have card
        if(userCard == null)
        {
            Owner.GrayScaleValue = 1f;
            Owner.AlphaValue = 0.7f;
            currentDisplayType = CardListElementDisplayType.E_NonOwned;
        }
        //non equit more card
        else if(alreadyInDeckCount == Mathf.Min(3, CardCount))
        {
            currentDisplayType = CardListElementDisplayType.E_NonMoreEquited;
            Owner.GrayScaleValue = 0f;
            Owner.AlphaValue = 0.3f;
        }
        else
        {
            Owner.GrayScaleValue = 0f;
            Owner.AlphaValue = 1f;
            currentDisplayType = CardListElementDisplayType.E_Idle;
        }
    }
}
