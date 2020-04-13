using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSlime : Character
{
    // Start is called before the first frame update
    void Start()
    {
        CurrentAction = CSlimeIdleAction.GetInstance();
    }
}
