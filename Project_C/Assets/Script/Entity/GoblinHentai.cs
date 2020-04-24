using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinHentai : Character
{
    // Start is called before the first frame update
    void Start()
    {
        CurrentAction = GoblinHentaiIdleAction.GetInstance();
    }
}
