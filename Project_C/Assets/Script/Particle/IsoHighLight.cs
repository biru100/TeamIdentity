using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IsoHighLight : IsoParticle
{
    public bool IsMove { get; set; }
    public Transform Target { get; set; }
    public Action<Vector3> AfterHighLightLogic { get; set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        _currentLifeTime += Time.deltaTime;

        if (_currentLifeTime >= LifeTime)
        {
            AfterHighLightLogic?.Invoke(transform.position);
            Destroy(gameObject);
        }

        if (IsMove && Target != null)
        {
            transform.position += ((Target.position + Vector3.down * Isometric.IsometricGridSize) - transform.position).normalized * 1f * Isometric.IsometricGridSize * Time.deltaTime;
        }
    }
}
