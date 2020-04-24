using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinStealer : Character
{
    void Start()
    {
        CurrentAction = GoblinStealerIdleAction.GetInstance();
    }
}
