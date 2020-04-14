using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTotem : Character
{
    // Start is called before the first frame update
    void Start()
    {
        CurrentAction = WoodTotemIdleAction.GetInstance();
    }
}
