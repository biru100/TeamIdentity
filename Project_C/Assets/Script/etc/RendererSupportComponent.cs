using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(RenderTransform))]
public class RendererSupportComponent : MonoBehaviour
{
    public static readonly string DepthShaderName = "Sprites/DepthSprite-Lit";

    [SerializeField] Texture _normalMap;

    RenderTransform _renderTransform;

    SpriteRenderer _renderer;
    MaterialPropertyBlock _propBlock;

    bool _isDepthTile = false;

    public bool IsChangeProperty { get; set; }
    public bool IsChangePosition { get; set; }

    void Update()
    {
        if(!Application.isPlaying)
            InputMaterialProperty();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        InputMaterialProperty();
    }

    void InputMaterialProperty()
    {
        if (_propBlock == null)
        {
            _propBlock = new MaterialPropertyBlock();
            IsChangeProperty = true;
        }

        if(_renderer == null)
            _renderer = GetComponent<SpriteRenderer>();

        if (_renderTransform == null)
            _renderTransform = GetComponent<RenderTransform>();

        if(IsChangeProperty)
        {
            _renderer.GetPropertyBlock(_propBlock);
            if (_renderer.sharedMaterial?.shader.name == DepthShaderName && _renderer.sprite != null)
            {
                Vector4 spriteRectData = new Vector4(_renderer.sprite.rect.x, _renderer.sprite.rect.y, _renderer.sprite.rect.width, _renderer.sprite.rect.height);
                _propBlock.SetVector("_SpriteRect", spriteRectData);
            }
            if (_normalMap != null)
                _propBlock.SetTexture("_BumpMap", _normalMap);
            _renderer.SetPropertyBlock(_propBlock);
        }

        if(_renderer.sharedMaterial?.shader.name == DepthShaderName)
        {
            _renderer.sortingOrder = Mathf.FloorToInt((-_renderTransform.z_weight + 1f) * 100f);
            IsChangePosition = false;
        }
        else
        {
            _renderer.sortingOrder = 300;
            IsChangePosition = false;
        }
    }
}
