using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Character
{
    public static Player CurrentPlayer { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        CurrentPlayer = this;
        Status = new PlayerStatus(this);
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

    protected void OnDestroy()
    {
        if (CurrentPlayer == this)
            CurrentPlayer = null;
    }
}
