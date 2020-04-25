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
}
