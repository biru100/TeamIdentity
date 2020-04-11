using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public enum CharacterNotifyType
{
    E_None,
    E_Damage,
    E_Dead
}

public class CharacterNotifyEvent
{
    public CharacterNotifyType Type { get; set; }
    public object Data { get; set; }

    public CharacterNotifyEvent(CharacterNotifyType type, object data)
    {
        Type = type;
        Data = data;
    }
}

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{
    public Animator Anim { get; protected set; }
    public NavMeshAgent NavAgent { get; protected set; }
    public CharacterStatus Status { get; protected set; }
    protected List<CharacterNotifyEvent>[] NotifyEventQueues { get; set; }
    protected int CurrentNotifyQueueIndex { get => _currentNotifyQueueIndex; set => _currentNotifyQueueIndex = value; }
    protected int NextNotifyQueueIndex { get => _currentNotifyQueueIndex; }

    protected int _currentNotifyQueueIndex;
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
        Anim = GetComponentInChildren<Animator>();
        NavAgent = GetComponent<NavMeshAgent>();
        NotifyEventQueues = new List<CharacterNotifyEvent>[2] 
        {
            new List<CharacterNotifyEvent>(),
            new List<CharacterNotifyEvent>()
        };
        _currentNotifyQueueIndex = 0;
    }

    protected virtual void Update()
    {
        CurrentAction?.UpdateAction();
        NotifyEventQueues[CurrentNotifyQueueIndex].Clear();
        CurrentNotifyQueueIndex = NextNotifyQueueIndex;
    }

    public virtual void AddNotifyEvent(CharacterNotifyEvent notifyEvent)
    {
        Status?.UpdateStatus(notifyEvent);
        NotifyEventQueues[CurrentNotifyQueueIndex].Add(notifyEvent);
    }

    public virtual void AddNextFrameNotifyEvent(CharacterNotifyEvent notifyEvent)
    {
        NotifyEventQueues[NextNotifyQueueIndex].Add(notifyEvent);
    }

    public virtual void ConsumeNotifyEvent(CharacterNotifyEvent notifyEvent)
    {
        NotifyEventQueues[CurrentNotifyQueueIndex].Remove(notifyEvent);
    }

    public virtual List<CharacterNotifyEvent> GetNotifyEvents()
    {
        return NotifyEventQueues[CurrentNotifyQueueIndex];
    }
}
