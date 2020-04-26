using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPDisplay : MonoBehaviour
{
    protected Image _hp;
    protected float _originSize;

    public static HPDisplay CreateHPDisplay()
    {
        HPDisplay display = Instantiate(ResourceManager.GetResource<GameObject>("UI/HPDisplay"), CanvasHelper.Main.transform).GetComponent<HPDisplay>();
        return display;
    }

    private void Awake()
    {
        _hp = GetComponentsInChildren<Image>()[1];
        _originSize = _hp.rectTransform.sizeDelta.x;
    }

    public void SetHPSize(float persent)
    {
        Vector2 size = _hp.rectTransform.sizeDelta;
        size.x = _originSize * persent;
        _hp.rectTransform.sizeDelta = size;
    }

    public void SetHPAmount(float persent)
    {
        _hp.fillAmount = persent;
    }
}
