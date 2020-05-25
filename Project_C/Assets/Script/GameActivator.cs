using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActivator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MainUIInterface.Instance.StartInterface();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if (MinimapInterface.Instance.gameObject.activeSelf)
                MinimapInterface.Instance.StartInterface();
            else
                MinimapInterface.Instance.StopInterface();
        }
    }

    public void StopGame()
    {
        Application.Quit();
    }
}
