using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorRangeViewer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 s = Isometric.IsometricRenderSize;
        transform.localRotation = Isometric.IsometricToWorldRotation;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
