using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{
    public RenderTransform RenderTrasform { get; protected set; }

    public Animator Anim { get; protected set; }
    public NavMeshAgent NavAgent { get; protected set; }
    public CharacterStatus Status { get; protected set; }

    public List<CharacterAbility> AbilityStack { get; protected set; }
    public List<CharacterState> StateStack { get; protected set; }

    protected List<CharacterState> DeleteStateList { get; set; }
    protected CharacterAction _currentAction;

    protected List<AbilityDisplay> _abilityDisplays;
    protected List<StateDisplay> _stateDisplays;


    public CharacterAction CurrentAction {
        get => _currentAction;
        set
        {
            if (_currentAction != null)
                _currentAction.FinishAction();
            _currentAction = value;
            value.StartAction(this);
        }
    }

    protected virtual void Awake()
    {
        Status = new CharacterStatus(this);
        StateStack = new List<CharacterState>();
        DeleteStateList = new List<CharacterState>();
        AbilityStack = new List<CharacterAbility>();

        _abilityDisplays = new List<AbilityDisplay>();
        _stateDisplays = new List<StateDisplay>();

        RenderTrasform = GetComponentInChildren<RenderTransform>();
        Anim = GetComponentInChildren<Animator>();
        NavAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        Status.PrepareState();
        for (int i = 0; i < StateStack.Count; ++i)
            StateStack[i].UpdateState();

        Status.PrepareAbility();
        for (int i = 0; i < AbilityStack.Count; ++i)
            AbilityStack[i].UpdateAbility();

        foreach (var deletedState in DeleteStateList)
            StateStack.Remove(deletedState);

        CurrentAction?.UpdateAction();

        NavAgent.speed = Status.CurrentSpeed;

        while(_abilityDisplays.Count < Status.CurrentAbility.Count)
        {
            _abilityDisplays.Add(AbilityDisplay.CreateAbilityDisplay());
        }

        while (_stateDisplays.Count < Status.CurrentStates.Count)
        {
            _stateDisplays.Add(StateDisplay.CreateStateDisplay());
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasHelper.Main.transform as RectTransform, 
            RectTransformUtility.WorldToScreenPoint(Camera.main, RenderTrasform.transform.position + Vector3.up * 0.3f), 
            CanvasHelper.Main.worldCamera, 
            out Vector2 displayCenterPosition);

        for (int i = 0; i < _abilityDisplays.Count; ++i)
        {
            if(i < Status.CurrentAbility.Count)
            {
                _abilityDisplays[i].gameObject.SetActive(true);
                (_abilityDisplays[i].transform as RectTransform).anchoredPosition = displayCenterPosition
                    + Vector2.right * (i - (Status.CurrentAbility.Count - 1) * 0.5f) * 60f;
                _abilityDisplays[i].Data = DataManager.GetData<AbilityTable>((int)Status.CurrentAbility[i]);
            }
            else
            {
                _abilityDisplays[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < _stateDisplays.Count; ++i)
        {
            if (i < Status.CurrentStates.Count)
            {
                _stateDisplays[i].gameObject.SetActive(true);
                (_stateDisplays[i].transform as RectTransform).anchoredPosition = displayCenterPosition
                    + Vector2.right * (i - (Status.CurrentStates.Count - 1) * 0.5f) * 60f + Vector2.up * 50f;
                _stateDisplays[i].Data = DataManager.GetData<StateTable>((int)Status.CurrentStates[i]);
            }
            else
            {
                _stateDisplays[i].gameObject.SetActive(false);
            }
        }
    }

    public virtual bool AddState(CharacterState state, bool isUnique = false)
    {
        if (Status.IgnoreStateList.Contains(state.StateType))
            return false;

        if (isUnique)
        {
            if (StateStack.Find((s) => s.StateType == state.StateType) == null)
            {
                StateStack.Add(state);
                return true;
            }
            return false;
        }
        else
        {
            StateStack.Add(state);
            return true;
        }
    }

    public virtual List<CharacterState> FindState(CharacterStateType type)
    {
        return StateStack.FindAll((s) => s.StateType == type);
    }

    public virtual void DeleteState(CharacterState state)
    {
        DeleteStateList.Add(state);
    }

    public virtual void DeleteState(CharacterStateType type)
    {
        DeleteStateList.Add(StateStack.FindLast((s)=>s.StateType == type));
    }

    protected virtual void OnDestroy()
    {
        foreach(var ability in _abilityDisplays)
        {
            if (ability != null)
                Destroy(ability.gameObject);
        }

        foreach (var state in _stateDisplays)
        {
            if (state != null)
                Destroy(state.gameObject);
        }
    }
}
