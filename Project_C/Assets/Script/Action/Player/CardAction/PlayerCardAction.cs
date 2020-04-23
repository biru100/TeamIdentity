using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTarget
{
    public Vector3 Point { get; set; }
    public Character Target { get; set; }

    public CardTarget() { }
    public CardTarget(Vector3 point) { Point = point; }
    public CardTarget(Character target) { Target = target; }
}

public class PlayerCardAction : CharacterAction
{
    public CardTarget Target { get; set; }

    public PlayerCardAction(CardTarget target)
    {
        Target = target;
    }
}
