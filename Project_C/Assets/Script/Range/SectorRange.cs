using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorRange : IRange
{
    public TargetData Target { get; set; }
    public float Angle { get; set; }
    public float Range { get; set; }

    public SectorRange(TargetData target, float angle, float range)
    {
        Target = target;
        Angle = angle;
        Range = range;
    }

    public Vector3 GetPosition()
    {
        return Target.type == TargetType.E_Character ? Target.Target.transform.position : Target.Point;
    }

    public List<Character> GetTargets()
    {
        throw new System.NotImplementedException();
    }
}
