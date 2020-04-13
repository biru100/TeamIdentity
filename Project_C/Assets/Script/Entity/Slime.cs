using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Character
{
    // Start is called before the first frame update
    void Start()
    {
        CurrentAction = SlimeIdleAction.GetInstance();
    }
}
