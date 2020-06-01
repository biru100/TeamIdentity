using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckViewer : MonoBehaviour
{
    public GameObject Content;
    public InputField DeckNameText;
    public Text DeckCardCountText;

    public DeckData ControlledDeckData { get; protected set; }

    protected List<DeckElement> DisplayDeckElementList;

    public void OnDeckNameChanged(string value)
    {
        ControlledDeckData.DeckName = DeckNameText.text;
    }

    public void SetControlledDeck(DeckData deckData = null)
    {
        if(deckData == null)
        {
            ControlledDeckData = new DeckData();
            UserData.Instance.OwnedDeckList.Add(ControlledDeckData);
            DeckNameText.text = ControlledDeckData.DeckName;
            DeckCardCountText.text = ControlledDeckData.CardCount + "/30";
        }
        else
        {
            ControlledDeckData = deckData;
            DeckNameText.text = ControlledDeckData.DeckName;
            DeckCardCountText.text = ControlledDeckData.CardCount + "/30";
        }

        if (DisplayDeckElementList != null)
        {
            foreach (var element in DisplayDeckElementList)
            {
                if (element != null)
                    Destroy(element.gameObject);
            }

            DisplayDeckElementList.Clear();
        }

        SettingDisplayElement();
    }

    void SettingDisplayElement()
    {
        if (DisplayDeckElementList == null)
        {
            DisplayDeckElementList = new List<DeckElement>();
        }

        while (DisplayDeckElementList.Count < ControlledDeckData.DeckCards.Count)
        {
            DisplayDeckElementList.Add(DeckElement.CreateDeckElement(Content.transform));
        }

        for (int i = 0; i < DisplayDeckElementList.Count; ++i)
        {
            DisplayDeckElementList[i].SetCardData(ControlledDeckData.DeckCards[i]);
        }

        SortingElement();
    }

    void SortingElement()
    {
        DisplayDeckElementList.Sort((e1, e2) => e1.CardData._Cost - e2.CardData._Cost);

        Vector3 basePos = new Vector3(174f, -10f, 0f);

        for (int i = 0; i < DisplayDeckElementList.Count; ++i)
        {
            DisplayDeckElementList[i].transform.localPosition = basePos + Vector3.down * 60f * i;
            DisplayDeckElementList[i].gameObject.SetActive(true);
        }

        Vector2 size = (Content.transform as RectTransform).sizeDelta;
        size.y = 60f * DisplayDeckElementList.Count + 30f;
        (Content.transform as RectTransform).sizeDelta = size;
    }

    public void AddCard(CardTable data)
    {
        if(ControlledDeckData.CardCount < 30)
        {
            UserCardData cardData = ControlledDeckData.DeckCards.Find((ucd) => ucd.cardIndex == data._Index);
            DeckElement element;
            if (cardData == null)
            {
                element = DeckElement.CreateDeckElement(Content.transform);
                cardData = new UserCardData(data._Index, 1);
                ControlledDeckData.DeckCards.Add(cardData);
                DisplayDeckElementList.Add(element);
                element.SetCardData(cardData);
                SortingElement();
            }
            else
            {
                cardData.cardCount++;
                element = DisplayDeckElementList.Find((de) => de.CardData == data);
                element.SetCardData(cardData);
            }
            ControlledDeckData.CardCount++;
            DeckCardCountText.text = ControlledDeckData.CardCount + "/30";
            //add anim
        }
    }

    public void RemoveCard(DeckElement element)
    {
        UserCardData cardData = ControlledDeckData.DeckCards.Find((ucd) => ucd.cardIndex == element.CardData._Index);
        if(cardData.cardCount == 1)
        {
            ControlledDeckData.DeckCards.Remove(cardData);
            DisplayDeckElementList.Remove(element);
            Destroy(element.gameObject); // temp
            SortingElement();
        }
        else
        {
            cardData.cardCount--;
            element.SetCardData(cardData);
        }
        ControlledDeckData.CardCount--;
        DeckCardCountText.text = ControlledDeckData.CardCount + "/30";
        //remove Anim

        DeckBuildingUIInterface.Instance.CardView.SettingCard();
    }

    public void CancelWork()
    {
        UserData.Instance.OwnedDeckList.Remove(ControlledDeckData);
        DeckBuildingUIInterface.Instance.Cancel();
    }

    public void FinishWork()
    {
        UserData.SaveFile();
        DeckBuildingUIInterface.Instance.Finish();
    }
}
