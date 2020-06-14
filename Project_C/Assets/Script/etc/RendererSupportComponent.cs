using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(RenderTransform))]
public class RendererSupportComponent : MonoBehaviour
{
    public static readonly string DepthShaderName = "Sprites/DepthSprite";
    public static readonly string DepthShaderLitName = "Sprites/DepthSprite-Lit";

    [SerializeField] Texture _normalMap;
    [SerializeField] Texture _depthTexture;
    [SerializeField] float _tileLength = 0.5888972f;
    [SerializeField] float _spriteYSize = 34;
    [SerializeField] float _depthColorMultiplier = 1f;

    RenderTransform _renderTransform;

    SpriteRenderer _renderer;
    MaterialPropertyBlock _propBlock;

    bool _isDepthTile = false;

    public bool IsChangeProperty { get; set; }
    public bool IsChangePosition { get; set; }

    void Update()
    {
        if (!Application.isPlaying)
        {
            IsChangeProperty = true;
            InputMaterialProperty();
        }
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
            if ((_renderer.sharedMaterial?.shader.name == DepthShaderName ||
                _renderer.sharedMaterial?.shader.name == DepthShaderLitName) && _renderer.sprite != null)
            {
                Vector4 spriteRectData = new Vector4(_renderer.sprite.rect.x, _renderer.sprite.rect.y, _renderer.sprite.rect.width, _renderer.sprite.rect.height);
                _propBlock.SetVector("_SpriteRect", spriteRectData);
            }
            if (_normalMap != null)
                _propBlock.SetTexture("_BumpMap", _normalMap);
            if(_depthTexture != null)
                _propBlock.SetTexture("_DepthTex", _depthTexture);

            _propBlock.SetFloat("_TileLength", _tileLength * _depthColorMultiplier);
            _propBlock.SetFloat("_SpriteYSize", _spriteYSize);

            _renderer.SetPropertyBlock(_propBlock);
        }

        if((_renderer.sharedMaterial?.shader.name == DepthShaderName ||
                _renderer.sharedMaterial?.shader.name == DepthShaderLitName))
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
