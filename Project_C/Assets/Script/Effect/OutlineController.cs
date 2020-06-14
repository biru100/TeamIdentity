using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OutlineController : MonoBehaviour
{
    [SerializeField] bool _activate = false;

    public SpriteRenderer Renderer { get; protected set; }

    public bool Activate { get=> _activate;
        set
        {
            Renderer.enabled = _activate = value;
        }
    }

    private void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Activate = false;
        Renderer.sortingOrder = 500;
    }
}
