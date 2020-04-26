using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    E_None,
    E_Character,
    E_Position
}

public class TargetData
{
    public TargetType type { get; protected set; }

    public Vector3 Point { get; protected set; }
    public Character Target { get; protected set; }

    public TargetData() { type = TargetType.E_None; }
    public TargetData(Vector3 point) { Point = point; type = TargetType.E_Position; }
    public TargetData(Character target) { Target = target; type = TargetType.E_Character; }

    public void SetTarget(Vector3 point) { Target = null; Point = point; type = TargetType.E_Position; }
    public void SetTarget(Character target) { Target = target; Point = Vector3.zero; type = TargetType.E_Character; }
}

public class PlayerCardAction : CharacterAction
{
    public CardTable DataTable { get; set; }
    public TargetData Target { get; set; }

    public PlayerCardAction(CardTable dataTable, TargetData target)
    {
        DataTable = dataTable;
        Target = target;
    }
}
