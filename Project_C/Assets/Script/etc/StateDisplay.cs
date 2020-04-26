using System.Collections;
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
                _image.sprite = ResourceManager.GetResource<Sprite>(value._IconStatePath);
            }
            _data = value;
        }
    }

    public static StateDisplay CreateStateDisplay()
    {
        StateDisplay display = Instantiate(ResourceManager.GetResource<GameObject>("UI/StateDisplay"), CanvasHelper.Main.transform).GetComponent<StateDisplay>();
        return display;
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
}
