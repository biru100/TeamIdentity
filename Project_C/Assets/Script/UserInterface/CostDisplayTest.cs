using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostDisplayTest : MonoBehaviour
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
        if (PlayerStatus.CurrentStatus == null)
            return;
        text.text = "Cost : " + PlayerStatus.CurrentStatus.CurrentManaCost;
    }
}
