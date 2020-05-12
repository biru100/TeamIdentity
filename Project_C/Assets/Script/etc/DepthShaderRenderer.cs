using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class DepthShaderRenderer : MonoBehaviour
{
    SpriteRenderer _renderer;
    MaterialPropertyBlock _propBlock;

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

        _renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        _renderer.receiveShadows = true;

        _renderer.GetPropertyBlock(_propBlock);
        Vector4 spriteRectData = new Vector4(_renderer.sprite.rect.x, _renderer.sprite.rect.y, _renderer.sprite.rect.width, _renderer.sprite.rect.height);
        //Debug.Log(spriteRectData);
        _propBlock.SetVector("_SpriteRect", spriteRectData);
        _renderer.SetPropertyBlock(_propBlock);
    }
}
