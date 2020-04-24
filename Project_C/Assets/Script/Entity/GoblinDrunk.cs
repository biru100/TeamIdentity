using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinDrunk : Character
{
    // Start is called before the first frame update
    void Start()
    {
        CurrentAction = GoblinDrunkIdleAction.GetInstance();
    }
}
