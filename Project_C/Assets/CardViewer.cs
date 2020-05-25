using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public enum CardTapType
{
    E_Owned,
    E_All
}

public enum CardCostType
{
    E_All,
    E_0,
    E_1,
    E_2,
    E_3,
    E_4,
    E_5,
    E_6,
    E_7Plus,
}

[RequireComponent(typeof(ScrollRect))]
public class CardViewer : MonoBehaviour
{
    public GameObject Content;
    public Text SearchText;
    public Dropdown TapDropdown;
    public Dropdown CostDropdown;

    protected List<CardInterface> CardList;
    protected List<CardTable> CardTableList;

    protected CardTapType CurrentTapOption = CardTapType.E_Owned;
    protected CardCostType CurrentCostOption = CardCostType.E_All;

    public void SetDisplay(bool isDisplay)
    {
        if(isDisplay)
        {
            SettingCard();
        }
    }

    public void SettingCard()
    {
        if(CardList == null)
        {
            CardList = new List<CardInterface>();
        }

        if (CardTableList == null)
            CardTableList = DataManager.GetDatas<CardTable>();

        if (Card.CardInstanceSet == null)
            Card.MakeCardInstanceSet(CardTableList);

        List<CardTable> displayList = CardTableList.Where(TapSectionClippingList).
            Where(CostSectionClippingList).
            Where(SearchTextClippingList).OrderBy((card)=>card._Cost).ThenBy((card)=>card._krName).ToList();

        Vector3 basePos = new Vector3(200f, -220f, 0f);

        while(CardList.Count < displayList.Count)
        {
            CardList.Add(CardInterface.CreateCard(Content.transform));
        }

        Vector2 size = (Content.transform as RectTransform).sizeDelta;
        size.y = ((displayList.Count / 4) + 1) * 500f;
        (Content.transform as RectTransform).sizeDelta = size;

        for (int i = 0; i < CardList.Count; ++i)
        {
            if(displayList.Count <= i)
            {
                CardList[i].gameObject.SetActive(false);
            }
            else
            {
                CardList[i].CardData = Card.CardInstanceSet[displayList[i]];
                CardList[i].CurrentAction = UICardListAction.GetInstance();
                (CardList[i].transform as RectTransform).anchorMin = new Vector2(0f, 1f);
                (CardList[i].transform as RectTransform).anchorMax = new Vector2(0f, 1f);
                CardList[i].transform.localPosition = basePos + Vector3.right * (i % 4) * 340f + Vector3.down * (i / 4) * 500f;
                CardList[i].gameObject.SetActive(true);
            }
        }
    }

    bool TapSectionClippingList(CardTable card)
    {
        if (CurrentTapOption == CardTapType.E_Owned)
        {
            return UserData.Instance.OwnedCardList.Find((usd) => usd.cardIndex == card._Index) != null;
        }
        else
            return true;
    }

    bool CostSectionClippingList(CardTable card)
    {
        if (CurrentCostOption == CardCostType.E_All)
        {
            return true;
        }
        else if(CurrentCostOption == CardCostType.E_7Plus)
            return card._Cost >= 7;
        else
            return card._Cost == (int)CurrentCostOption - 1;
    }

    bool SearchTextClippingList(CardTable card)
    {
        if (SearchText.text.Length == 0)
        {
            return true;
        }
        else
        {
            return card._krName.IndexOf(SearchText.text, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }

    public void TapDropDownValueChanged(int value)
    {
        CurrentTapOption = (CardTapType)int.Parse(TapDropdown.options[value].text);
        SettingCard();
    }

    public void CostDropDownValueChanged(int value)
    {
        CurrentCostOption = (CardCostType)int.Parse(CostDropdown.options[value].text);
        SettingCard();
    }

    public void SearchTextChanged(string value)
    {
        SettingCard();
    }

}
