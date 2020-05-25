using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(RenderTransform))]
public class RendererSupportComponent : MonoBehaviour
{
    public static readonly string DepthShaderName = "Sprites/DepthSprite";

    RenderTransform _renderTransform;

    SpriteRenderer _renderer;
    MaterialPropertyBlock _propBlock;

    bool _isDepthTile = false;

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
        if(_propBlock == null)
            _propBlock = new MaterialPropertyBlock();

        if(_renderer == null)
            _renderer = GetComponent<SpriteRenderer>();

        if (_renderTransform == null)
            _renderTransform = GetComponent<RenderTransform>();

        if(_renderer.sharedMaterial?.shader.name == DepthShaderName)
        {
            _renderer.sortingOrder = Mathf.FloorToInt((-_renderTransform.z_weight + 1f) * 100f);
            _renderer.GetPropertyBlock(_propBlock);
            Vector4 spriteRectData = new Vector4(_renderer.sprite.rect.x, _renderer.sprite.rect.y, _renderer.sprite.rect.width, _renderer.sprite.rect.height);

            _propBlock.SetVector("_SpriteRect", spriteRectData);
            _renderer.SetPropertyBlock(_propBlock);
        }
        else
        {
            _renderer.sortingOrder = 300;
        }
    }
}
