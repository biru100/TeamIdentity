using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{
    public Rigidbody Body { get; protected set; }
    public Animator Anim { get; protected set; }
    public NavMeshAgent NavAgent { get; protected set; }

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
        Body = GetComponent<Rigidbody>();
        Anim = GetComponentInChildren<Animator>();
        NavAgent = GetComponent<NavMeshAgent>();
    }
    
}
