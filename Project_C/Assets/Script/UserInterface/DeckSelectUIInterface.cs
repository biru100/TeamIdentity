using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSelectUIInterface : ManagedUIInterface<DeckSelectUIInterface>
{
    public DeckUI[] DeckUIs;
    public GameObject PrevButton;
    public GameObject NextButton;

    public int CurrentPage { get; set; }

    public override void StartInterface()
    {
        base.StartInterface();
        CurrentPage = 0;
        SettingPage(CurrentPage);
    }

    void SettingPage(int page)
    {
        int StartIndex = page * 8;
        List<DeckData> deckDatas = UserData.Instance.OwnedDeckList;
        if(deckDatas.Count > (page + 1) * 8)
        {
            NextButton.SetActive(true);
        }
        else
        {
            NextButton.SetActive(false);
        }

        if (0 <= (page - 1) * 8)
        {
            PrevButton.SetActive(true);
        }
        else
        {
            PrevButton.SetActive(false);
        }


        for(int i = 0; i < 8; ++i)
        {
            if (deckDatas.Count > i)
                DeckUIs[i].SetDeckData(deckDatas[i]);
            else
                DeckUIs[i].SetDeckData(null);
        }
    }

    public void Next()
    {
        CurrentPage++;
        SettingPage(CurrentPage);
    }

    public void Prev()
    {
        CurrentPage--;
        SettingPage(CurrentPage);
    }

    public void GotoPlay()
    {
        StopInterface();

        InGameInterface.Instance.StartInterface();
        RoomManager.CreateRogueMap();
        RoomManager.CreatePlayer();
    }

    public void GotoDeckBuilding(DeckData data)
    {
        DeckBuildingUIInterface.Instance.CurrentDeckData = data;
        StopInterface();
        DeckBuildingUIInterface.Instance.StartInterface();
    }
}
