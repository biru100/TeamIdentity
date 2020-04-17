using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // Start is called before the first frame update
    void Start()
    {
        Status.Hp = 1000000000f;
        Status.CurrentHp = Status.Hp;
        CurrentAction = EnemyIdleAction.GetInstance();
    }
}
