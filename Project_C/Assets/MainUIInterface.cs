using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIInterface : ManagedUIInterface<MainUIInterface>
{ 
    public void StartGame()
    {
        StopInterface();
        DeckSelectUIInterface.Instance.StartInterface();
    }

    public void GoSetting()
    {

    }

    public void FinishGame()
    {
        Application.Quit();
    }
}
