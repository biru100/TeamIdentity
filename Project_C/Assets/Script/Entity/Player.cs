﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Character
{
    public static Player CurrentPlayer { get; protected set; }

    public List<Card> UseCardStack { get; set; }

    protected override void Awake()
    {
        base.Awake();
        CurrentPlayer = this;
        Status = new PlayerStatus(this);
        UseCardStack = new List<Card>();
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
            EntityUtil.ChangeAction(this, UseCardStack[0].CardActionName);
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
