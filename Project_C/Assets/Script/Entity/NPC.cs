using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Character
{
    [SerializeField] protected string _systemName;
    public string SystemName { get => _systemName; }
    public NPCTable Data { get; protected set; }
    public bool IsPersistenceAbility { get; set; }
    public bool IsUse { get; set; }
    public bool CanUse {
        get
        {
            return IsPersistenceAbility || !IsUse;
        }
    }

    public bool IsCalledInteraction { get; set; }

    protected override void Awake()
    {
        RenderTrasform = GetComponentInChildren<RenderTransform>();
        Anim = GetComponentInChildren<Animator>();
        NavAgent = GetComponent<NavMeshAgent>();

        Data = DataManager.GetDatas<NPCTable>().Find((s)=>s._Name == _systemName);
        IsPersistenceAbility = Data._PersistenceAbility;
    }

    protected void Start()
    {
        if (CurrentAction == null)
            EntityUtil.ChangeAction(this, Data._Name + "IdleAction");
    }

    protected override void Update()
    {
        CurrentAction?.UpdateAction();
    }
}
