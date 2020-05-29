using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRangeInterface : MonoBehaviour
{
    protected static CardRangeInterface _instance;

    public static CardRangeInterface Instance
    {
        get
        {
            if (_instance == null)
            {
                CardRangeInterface instance = FindObjectOfType<CardRangeInterface>();
                if (instance == null)
                    instance = Instantiate(ResourceManager.GetResource<GameObject>("Ranges/" + typeof(CardRangeInterface).Name))
                        .GetComponent<CardRangeInterface>();

                _instance = instance;
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null || _instance == this)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        RenderChild = GetComponentInChildren<RenderTransform>();
        Renderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public RenderTransform RenderChild { get; set; }
    public SpriteRenderer Renderer { get; set; }
    public CardRangeType RangeType { get; protected set; }
    public Vector3 CurrentPoint { get; set; }
    public string CardSpriteName { get; protected set; }

    public bool IsVisible { get; set; }

    bool NeedUpdate { get; set; }
    int CurrentAngle { get; set; }
    Sprite CurrentSprite { get; set; }

    public static void SetRangeInterface(CardTable cardData)
    {
        Instance.RangeType = cardData._RangeType;
        Instance.CardSpriteName = cardData._CardRangeSprite;
        Instance.NeedUpdate = true;
        Instance.IsVisible = false;
    }

    void UpdateSprite()
    {
        if (RangeType == CardRangeType.E_PlayerRelativeCircularSector)
        {
            Renderer.sprite = ResourceManager.GetResource<Sprite>(CardSpriteName + "_" + CurrentAngle);
        }
        if (RangeType == CardRangeType.E_PlayerRelativeCircle)
        {
            Renderer.sprite = ResourceManager.GetResource<Sprite>(CardSpriteName);
        }
        else if(RangeType == CardRangeType.E_PointCircle)
        {
            Renderer.sprite = ResourceManager.GetResource<Sprite>(CardSpriteName);
        }
    }

    private void Update()
    {
        if(RangeType != CardRangeType.E_None && IsVisible)
        {
            if(RangeType == CardRangeType.E_PlayerRelativeCircularSector)
            {
                transform.position = Player.CurrentPlayer.transform.position + Vector3.down * Isometric.IsometricGridSize;

                int newAngle = EffectiveUtility.GetMouseAngle(Player.CurrentPlayer.transform);
                if (newAngle != CurrentAngle || NeedUpdate)
                {
                    CurrentAngle = newAngle;
                    UpdateSprite();
                    NeedUpdate = false;
                }
            }
            else if(RangeType == CardRangeType.E_PlayerRelativeCircle)
            {
                transform.position = Player.CurrentPlayer.transform.position + Vector3.down * Isometric.IsometricGridSize;
                if (NeedUpdate)
                {
                    UpdateSprite();
                    NeedUpdate = false;
                }

            }
            else if(RangeType == CardRangeType.E_PointCircle)
            {
                transform.position = CurrentPoint + Vector3.down * Isometric.IsometricGridSize;
                if(NeedUpdate)
                {
                    UpdateSprite();
                    NeedUpdate = false;
                }
            }
            Renderer.enabled = true;
        }
        else
        {
            Renderer.enabled = false;
            NeedUpdate = true;
        }
    }
}
