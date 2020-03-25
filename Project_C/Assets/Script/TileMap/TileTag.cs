using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileTag : MonoBehaviour
{
    public string Tag;

    void Update()
    {
        if (!Application.IsPlaying(gameObject))
            Tag = gameObject.name;
    }
}
