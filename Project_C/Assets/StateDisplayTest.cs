using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StateDisplayTest : MonoBehaviour
{
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "";
        foreach (var state in PlayerStatus.CurrentStatus.CurrentStates)
        {
            text.text += Enum.GetName(state.GetType(), state) + "\n";
        }
    }
}
