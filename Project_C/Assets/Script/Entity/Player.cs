using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Character
{
    public static Player CurrentPlayer { get; protected set; }

    public List<KeyValuePair<Card, CardTarget>> UseCardStack { get; set; }

    protected override void Awake()
    {
        base.Awake();
        CurrentPlayer = this;
        Status = new PlayerStatus(this);
        DataManager.LoadData();
        UseCardStack = new List<KeyValuePair<Card, CardTarget>>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentAction = PlayerIdleAction.GetInstance();

        for(int i = 0; i < 30; ++i)
        {
            Deck.Instance.AddCard(new Card(Random.Range(0, 3)));
        }

        InGameInterface.Instance.DrawCard();
        InGameInterface.Instance.DrawCard();
        InGameInterface.Instance.DrawCard();
        InGameInterface.Instance.DrawCard();
    }

    protected override void Update()
    {
        Status.PrepareState();
        for (int i = 0; i < StateStack.Count; ++i)
            StateStack[i].UpdateState();

        foreach (var deletedState in DeleteStateList)
            StateStack.Remove(deletedState);

        if (UseCardStack.Count != 0)
        {
            EntityUtil.ChangeCardAction(this, UseCardStack[0].Key.CardActionName, UseCardStack[0].Value);
            UseCardStack.RemoveAt(0);
        }
        else
            CurrentAction?.UpdateAction();
    }

    protected void OnDestroy()
    {
        if (CurrentPlayer == this)
            CurrentPlayer = null;
    }
}
