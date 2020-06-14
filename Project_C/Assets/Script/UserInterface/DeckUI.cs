using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckUI : MonoBehaviour
{
    public Image DeckImage;
    public Text DeckNameText;

    public GameObject HighlightImage;
    public GameObject CreateButton;
    public GameObject EditButton;
    public GameObject SelectButton;

    public DeckData DeckDataInstance { get; protected set; }

    public void SetDeckData(DeckData data)
    {
        DeckDataInstance = data;

        HighlightImage.SetActive(false);
        if(data == null)
        {
            CreateButton.SetActive(true);
            EditButton.SetActive(false);
            SelectButton.SetActive(false);

            DeckNameText.text = "";
        }
        else
        {
            CreateButton.SetActive(false);
            EditButton.SetActive(true);
            SelectButton.SetActive(true);

            DeckNameText.text = DeckDataInstance.DeckName;
        }
    }

    public void EditDeck()
    {
        DeckSelectUIInterface.Instance.GotoDeckBuilding(DeckDataInstance);
    }

    public void CreateDeck()
    {
        DeckSelectUIInterface.Instance.GotoDeckBuilding(DeckDataInstance);
    }

    public void SelectDeck()
    {
        DeckManager.LoadDeck(DeckDataInstance.DeckName);
        DeckSelectUIInterface.Instance.GotoPlay();
    }
}
