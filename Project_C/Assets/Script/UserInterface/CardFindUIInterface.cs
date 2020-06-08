using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardFindUIInterface : ManagedUIInterface<CardFindUIInterface>
{
    public List<CardTable> FindList { get; set; }
    public Action FinishAction { get; set; }

    public List<CardInterface> CardList { get; set; }

    public override void StartInterface()
    {
        base.StartInterface();

        if(InGameInterface.Instance.MaxHandCardCount == InGameInterface.Instance.CurrentHandCardCount
            || FindList == null)
        {
            gameObject.SetActive(false);
            IsActive = false;
            return;
        }

        CardList = new List<CardInterface>();

        foreach (var data in FindList)
        {
            CardInterface ci = CardInterface.CreateCard(transform);
            CardList.Add(ci);
            ci.CardData = Card.CardInstanceSet[data];
            ci.HandIndex = CardList.Count - 1;
            ci.CurrentAction = FindCardAction.GetInstance();

            ci.transform.localPosition = Vector3.zero + new Vector3(400f * (ci.HandIndex - (FindList.Count - 1) * 0.5f) * 0.5f, 0f, 0f);
        }
    }

    public override void StopInterface()
    {
        StartCoroutine(HookAnimationFinished("stop", () => { IsActive = false; } + FinishAction));
    }

    public void SelectCard(CardInterface ci)
    {
        InGameInterface.Instance.AddToHand(ci);
        
        foreach(var card in CardList)
        {
            if(card != ci)
            {
                Destroy(card.gameObject);
            }
        }

        CardList = null;
        StopInterface();
    }
}
