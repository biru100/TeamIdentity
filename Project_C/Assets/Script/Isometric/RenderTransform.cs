using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(RendererSupportComponent))]
public class RenderTransform : MonoBehaviour
{
    [SerializeField] protected Vector3 _imageOffset = Vector3.zero;
    [SerializeField] protected Quaternion _rotation = Quaternion.identity;
    protected Vector3 _lastParentPosition = Vector3.zero;

    public Vector3 ImageOffset { get => _imageOffset;
        set
        {
            _imageOffset = value;
            RendererSupporter.IsChangePosition = true;
        }
    }

    public Quaternion Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            RendererSupporter.IsChangePosition = true;
        }
    }

    public float z_weight = 0f;

    RendererSupportComponent _rsComponent = null;
    public RendererSupportComponent RendererSupporter {
        get
        {
            if (_rsComponent == null)
                _rsComponent = GetComponent<RendererSupportComponent>();
            return _rsComponent;
        }
    }

    private void Update()
    {
        if(!Application.isPlaying)
            TranslateIsometricToWorldCoordination(true);
    }

    private void LateUpdate()
    {
        TranslateIsometricToWorldCoordination();
    }

    public Vector3 GetIsometricPosition()
    {
        TranslateIsometricToWorldCoordination();
        return transform.position;
    }

    public void TranslateIsometricToWorldCoordination(bool isForcedUpdate = false)
    {
        if (!isForcedUpdate && _lastParentPosition == transform.parent.position)
            return;

        _lastParentPosition = transform.parent.position;
        RendererSupporter.IsChangePosition = true;

        transform.position = Isometric.IsometricToWorldRotation * transform.parent.position
            + _imageOffset
            + Vector3.forward * Isometric.IsometricTileSize.z * z_weight * 0.5f;

        //transform.position = new Vector3(transform.position.x
        //, transform.position.y, transform.position.z);

        transform.position = new Vector3(Mathf.Round(transform.position.x * 100f) * 0.01f
        , Mathf.Round(transform.position.y * 100f) * 0.01f, transform.position.z);

        transform.rotation = _rotation;
    }
}
