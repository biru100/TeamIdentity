using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckElement : MonoBehaviour , IPointerClickHandler
{
    public static DeckElement CreateDeckElement(Transform parent)
    {
        return Instantiate(ResourceManager.GetResource<GameObject>("UI/Deck/DeckElement"), parent).GetComponent<DeckElement>();
    }

    [SerializeField] protected Text _costUI;
    [SerializeField] protected Image _cardImg;
    [SerializeField] protected Text _cardName;
    [SerializeField] protected Text _cardCountUI;

    CardTable _cardData;
    public CardTable CardData { get => _cardData; }

    public void SetCardData(UserCardData cardData)
    {
        _cardData = DataManager.GetData<CardTable>(cardData.cardIndex);
        _costUI.text = _cardData._Cost.ToString();
        _cardName.text = _cardData._krName;
        _cardCountUI.text = cardData.cardCount.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DeckBuildingUIInterface.Instance.DeckView.RemoveCard(this);
    }
}
