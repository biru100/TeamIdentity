using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AbilityDisplay : MonoBehaviour
{
    protected AbilityTable _data;

    Image _image;

    public AbilityTable Data { get => _data;
        set
        {
            if(_data != value)
            {
                _image.sprite = ResourceManager.GetResource<Sprite>(value._StatePath);
            }
            _data = value;
        }
    }

    public static AbilityDisplay CreateAbilityDisplay()
    {
        AbilityDisplay display= Instantiate(ResourceManager.GetResource<GameObject>("UI/AbilityDisplay"), CanvasHelper.Main.transform).GetComponent<AbilityDisplay>();
        return display;
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
}
