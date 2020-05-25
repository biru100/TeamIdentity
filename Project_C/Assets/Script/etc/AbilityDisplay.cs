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
                if(_image == null)
                    _image = GetComponent<Image>();
                _image.sprite = ResourceManager.GetResource<Sprite>(value._StatePath);
            }
            _data = value;
        }
    }

    public static AbilityDisplay CreateAbilityDisplay(Transform parent)
    {
        AbilityDisplay display= Instantiate(ResourceManager.GetResource<GameObject>("UI/AbilityDisplay"), parent).GetComponent<AbilityDisplay>();
        return display;
    }
}
