using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IsometricTransform))]
public class IsometricTileGuideLine : MonoBehaviour
{
    public Vector3 guideSize = Vector3.one;
    public bool visible = true;

    private void OnDrawGizmos()
    {
        if (!visible)
            return;

        IsometricTransform itransform = GetComponent<IsometricTransform>();
        Vector3 boxSize = EffectiveUtility.VectorMultiple(guideSize, Isometric.IsometricTileSize);

        Vector3[] box = new Vector3[]
        {
            itransform.position + EffectiveUtility.VectorMultiple(new Vector3(-0.5f, 1f, 0.5f), boxSize),
            itransform.position + EffectiveUtility.VectorMultiple(new Vector3(-0.5f, 1f, -0.5f), boxSize),
            itransform.position + EffectiveUtility.VectorMultiple(new Vector3(0.5f, 1f, -0.5f), boxSize),
            itransform.position + EffectiveUtility.VectorMultiple(new Vector3(0.5f, 1f, 0.5f), boxSize),
            itransform.position + EffectiveUtility.VectorMultiple(new Vector3(-0.5f, 0f, -0.5f), boxSize),
            itransform.position + EffectiveUtility.VectorMultiple(new Vector3(0.5f, 0f, -0.5f), boxSize),
            itransform.position + EffectiveUtility.VectorMultiple(new Vector3(0.5f, 0f, 0.5f), boxSize)
        };

        Gizmos.color = new Color(0.1f, 0.9f, 0.1f);
        Gizmos.DrawLine(Isometric.TranslationIsometricToScreen(box[0]), Isometric.TranslationIsometricToScreen(box[1]));
        Gizmos.DrawLine(Isometric.TranslationIsometricToScreen(box[1]), Isometric.TranslationIsometricToScreen(box[2]));
        Gizmos.DrawLine(Isometric.TranslationIsometricToScreen(box[2]), Isometric.TranslationIsometricToScreen(box[3]));
        Gizmos.DrawLine(Isometric.TranslationIsometricToScreen(box[3]), Isometric.TranslationIsometricToScreen(box[0]));

        Gizmos.DrawLine(Isometric.TranslationIsometricToScreen(box[1]), Isometric.TranslationIsometricToScreen(box[4]));
        Gizmos.DrawLine(Isometric.TranslationIsometricToScreen(box[2]), Isometric.TranslationIsometricToScreen(box[5]));
        Gizmos.DrawLine(Isometric.TranslationIsometricToScreen(box[3]), Isometric.TranslationIsometricToScreen(box[6]));

        Gizmos.DrawLine(Isometric.TranslationIsometricToScreen(box[4]), Isometric.TranslationIsometricToScreen(box[5]));
        Gizmos.DrawLine(Isometric.TranslationIsometricToScreen(box[5]), Isometric.TranslationIsometricToScreen(box[6]));

        Gizmos.color = Color.white;
    }
}
