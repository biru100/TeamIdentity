using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPDisplayTest : MonoBehaviour
{
    Text text;
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
        text.text = PlayerStatus.CurrentStatus.CurrentArmor + "\n" + PlayerStatus.CurrentStatus.CurrentHp + " / " + PlayerStatus.CurrentStatus.Hp;
    }
}
