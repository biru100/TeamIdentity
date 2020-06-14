using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RendererSupportType
{
    E_None,
    E_Character,
    E_NonDepthShader,
    E_DepthShaderLit,
    E_DepthShader
}

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(RenderTransform))]
public class RendererSupportComponent : MonoBehaviour
{
    public static readonly string DepthShaderName = "Sprites/DepthSprite";
    public static readonly string DepthShaderLitName = "Sprites/DepthSprite-Lit";

    [SerializeField] RendererSupportType _supportType = RendererSupportType.E_None;
    [SerializeField] bool _isStatic = false;
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
            IsChangePosition = true;
            InputMaterialProperty();
        }
        else
        {
            InputMaterialProperty();
        }
    }

    void InputMaterialProperty()
    {

        if (_propBlock == null)
        {
            _propBlock = new MaterialPropertyBlock();
            IsChangeProperty = true;
            IsChangePosition = true;
        }

        if(_renderer == null)
            _renderer = GetComponent<SpriteRenderer>();

        if (_supportType == RendererSupportType.E_None)
        {
            switch (_renderer.sharedMaterial?.shader.name)
            {
                case "Sprites/DepthSprite":
                    _supportType = RendererSupportType.E_DepthShader;
                    break;
                case "Sprites/DepthSprite-Lit":
                    _supportType = RendererSupportType.E_DepthShaderLit;
                    break;
                case "Sprites/NonDepthSprite-Lit":
                    _supportType = RendererSupportType.E_Character;
                    break;
                case "Sprites/NonDepthSprite":
                    _supportType = RendererSupportType.E_NonDepthShader;
                    break;
                default:
                    break;
            }
        }

        if (_renderTransform == null)
            _renderTransform = GetComponent<RenderTransform>();

        if (IsChangeProperty)
        {
            switch (_supportType)
            {
                case RendererSupportType.E_Character:
                    {
                        _renderer.GetPropertyBlock(_propBlock);
                        if(_normalMap != null) _propBlock.SetTexture("_BumpMap", _normalMap);
                        _renderer.SetPropertyBlock(_propBlock);
                    }
                    break;
                case RendererSupportType.E_NonDepthShader:
                    break;

                case RendererSupportType.E_DepthShader:
                    {
                        _renderer.GetPropertyBlock(_propBlock);
                        Vector4 spriteRectData = new Vector4(_renderer.sprite.rect.x, _renderer.sprite.rect.y, _renderer.sprite.rect.width, _renderer.sprite.rect.height);
                        _propBlock.SetVector("_SpriteRect", spriteRectData);
                        if (_depthTexture != null) _propBlock.SetTexture("_DepthTex", _depthTexture);
                        _propBlock.SetFloat("_TileLength", _tileLength * _depthColorMultiplier);
                        _propBlock.SetFloat("_SpriteYSize", _spriteYSize);
                        _renderer.SetPropertyBlock(_propBlock);
                    }
                    break;
                case RendererSupportType.E_DepthShaderLit:
                    {
                        _renderer.GetPropertyBlock(_propBlock);
                        if (_renderer.sprite != null)
                        {
                            Vector4 spriteRectData = new Vector4(_renderer.sprite.rect.x, _renderer.sprite.rect.y, _renderer.sprite.rect.width, _renderer.sprite.rect.height);
                            _propBlock.SetVector("_SpriteRect", spriteRectData);
                        }
                        if (_normalMap != null) _propBlock.SetTexture("_BumpMap", _normalMap);
                        if (_depthTexture != null) _propBlock.SetTexture("_DepthTex", _depthTexture);
                        _propBlock.SetFloat("_TileLength", _tileLength * _depthColorMultiplier);
                        _propBlock.SetFloat("_SpriteYSize", _spriteYSize);
                        _renderer.SetPropertyBlock(_propBlock);
                    }
                    break;
            }

            IsChangeProperty = false || !_isStatic;
        }

        if (IsChangePosition)
        {
            if (_supportType == RendererSupportType.E_DepthShader
                || _supportType == RendererSupportType.E_DepthShaderLit)
            {
                _renderer.sortingOrder = Mathf.FloorToInt((-_renderTransform.z_weight + 1f) * 100f);
                IsChangePosition = false || !_isStatic;
            }
            else
            {
                _renderer.sortingOrder = 300;
                IsChangePosition = false;
            }
        }
    }
}
