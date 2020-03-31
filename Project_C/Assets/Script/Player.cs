using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Character
{
    // Start is called before the first frame update
    void Start()
    {
        CurrentAction = PlayerIdleAction.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentAction?.UpdateAction();
    }
}
