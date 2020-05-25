using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuildingUIInterface : ManagedUIInterface<DeckBuildingUIInterface>
{
    public CardViewer CardView;
    public DeckViewer DeckView;

    public DeckData CurrentDeckData { get; set; }

    protected override void Awake()
    {
        base.Awake();
    }

    public override void StartInterface()
    {
        base.StartInterface();
        DeckView.SetControlledDeck(CurrentDeckData);
        CardView.SettingCard();
    }

    public void Cancel()
    {
        StopInterface();
        DeckSelectUIInterface.Instance.StartInterface();
    }

    public void Finish()
    {
        StopInterface();
        DeckSelectUIInterface.Instance.StartInterface();
    }
}
