using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHUD : MonoBehaviour
{
    [SerializeField] protected Image _hp;
    public Character Owner { get; protected set; }
    public Image HPImage { get => _hp; }

    protected List<AbilityDisplay> _abilityDisplays;
    protected List<StateDisplay> _stateDisplays;

    public static CharacterHUD CreateHUD(Character owner)
    {
        CharacterHUD display = Instantiate(ResourceManager.GetResource<GameObject>("UI/HUD"), CanvasHelper.Main.transform).GetComponent<CharacterHUD>();
        display.Owner = owner;
        return display;
    }

    private void Awake()
    {
        _abilityDisplays = new List<AbilityDisplay>();
        _stateDisplays = new List<StateDisplay>();
    }

    private void LateUpdate()
    {
        if(Owner.gameObject.activeInHierarchy == false)
        {
            gameObject.SetActive(false);
            return;
        }

        while (_abilityDisplays.Count < Owner.Status.CurrentAbility.Count)
        {
            _abilityDisplays.Add(AbilityDisplay.CreateAbilityDisplay(transform));
        }

        while (_stateDisplays.Count < Owner.Status.CurrentStates.Count)
        {
            _stateDisplays.Add(StateDisplay.CreateStateDisplay(transform));
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasHelper.Main.transform as RectTransform,
            RectTransformUtility.WorldToScreenPoint(Camera.main, Owner.RenderTrasform.GetIsometricPosition()
            + Vector3.up * Isometric.IsometricGridSize * Owner.HUDOffset),
            CanvasHelper.Main.worldCamera,
            out Vector2 displayCenterPosition))
        {
            Vector2 lastPos = (transform as RectTransform).anchoredPosition;
            (transform as RectTransform).anchoredPosition = displayCenterPosition;

            //if((lastPos - displayCenterPosition + Vector2.up * Owner.HUDOffset).magnitude > 1000f)
            //{
            //    int a = 0;
            //}
        }

        SetHPAmount(Owner.Status.CurrentHp / Owner.Status.Hp);

        for (int i = 0; i < _abilityDisplays.Count; ++i)
        {
            if (i < Owner.Status.CurrentAbility.Count)
            {
                (_abilityDisplays[i].transform as RectTransform).anchoredPosition = 
                    Vector2.right * (i - (Owner.Status.CurrentAbility.Count - 1) * 0.5f) * 60f;
                if (_abilityDisplays[i].Data == null || _abilityDisplays[i].Data._Index != (int)Owner.Status.CurrentAbility[i])
                    _abilityDisplays[i].Data = DataManager.GetData<AbilityTable>((int)Owner.Status.CurrentAbility[i]);

                _abilityDisplays[i].gameObject.SetActive(true);
            }
            else
            {
                _abilityDisplays[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < _stateDisplays.Count; ++i)
        {
            if (i < Owner.Status.CurrentStates.Count)
            {
                (_stateDisplays[i].transform as RectTransform).anchoredPosition = 
                    Vector2.right * (i - (Owner.Status.CurrentStates.Count - 1) * 0.5f) * 60f + Vector2.up * 50f;
                if(_stateDisplays[i].Data == null || _stateDisplays[i].Data._Index != (int)Owner.Status.CurrentStates[i])
                    _stateDisplays[i].Data = DataManager.GetData<StateTable>((int)Owner.Status.CurrentStates[i]);

                _stateDisplays[i].gameObject.SetActive(true);
            }
            else
            {
                _stateDisplays[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetHPAmount(float persent)
    {
        HPImage.fillAmount = persent;
    }
}
