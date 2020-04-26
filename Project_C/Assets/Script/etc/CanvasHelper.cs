using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasHelper : MonoBehaviour
{
    public static Canvas Main { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        Main = GetComponent<Canvas>();
    }
}
