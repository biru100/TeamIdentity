using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinStore : Character
{
    // Start is called before the first frame update
    void Start()
    {
        CurrentAction = GoblinStoreIdleAction.GetInstance();
    }
}
