using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRange
{
    List<Character> GetTargets();
    Vector3 GetPosition();
}
