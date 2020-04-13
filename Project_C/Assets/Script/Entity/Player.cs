using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Character
{
    public static Player CurrentPlayer { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        CurrentPlayer = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentAction = PlayerIdleAction.GetInstance();
        //Card card = new Card("Power Attack", "범위내 적에게 _ 만큼의 데미지를 준다.", new List<float>() { 100f }, 
        //    ResourceManager.GetResource<Sprite>("Sprites/"))
    }
}
