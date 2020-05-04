using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActivator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RoomManager.Instance.CreatePlayer();
        MinimapInterface.Instance.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            MinimapInterface.Instance.gameObject.SetActive(!MinimapInterface.Instance.gameObject.activeSelf);
        }
    }
}
