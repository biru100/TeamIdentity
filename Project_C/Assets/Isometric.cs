using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isometric
{
    private static readonly Quaternion _isometricToWorldRotation = new Quaternion(0.2798482f, 0.3647052f, 0.1159169f, 0.8804762f);
    private static readonly Quaternion _worldToIsometricRotation = new Quaternion(-0.2798482f, -0.3647052f, -0.1159169f, 0.8804762f);

    public static Vector2 _isometricTileSize = Vector2.one;

    public static Quaternion WorldToIsometricRotation
    {
        get
        {
            return _worldToIsometricRotation;
        }
    }

    public static Quaternion IsometricToWorldRotation
    {
        get
        {
            return _isometricToWorldRotation;
        }
    }
}
