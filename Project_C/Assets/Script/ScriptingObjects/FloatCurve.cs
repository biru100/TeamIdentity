using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable, CreateAssetMenu(fileName = "New Float Curve", menuName = "Curve/Float Curve")]
public class FloatCurve : ScriptableObject
{
    public static FloatCurve GetCurve(string path)
    {
        return ResourceManager.GetResource<FloatCurve>(path);
    }

    [SerializeField] protected AnimationCurve _curve;
    public AnimationCurve Curve { get => _curve; set => _curve = value; }

    public float Evaluate(float time)
    {
        return Curve.Evaluate(time);
    }

}
