﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class StateDisplay : MonoBehaviour
{
    protected StateTable _data;

    Image _image;

    public StateTable Data { get => _data;
        set
        {
            if(_data != value)
            {
                if (_image == null)
                    _image = GetComponent<Image>();
                _image.sprite = ResourceManager.GetResource<Sprite>(value._IconStatePath);
            }
            _data = value;
        }
    }

    public static StateDisplay CreateStateDisplay(Transform parent)
    {
        StateDisplay display = Instantiate(ResourceManager.GetResource<GameObject>("UI/StateDisplay"), parent).GetComponent<StateDisplay>();
        return display;
    }
}
