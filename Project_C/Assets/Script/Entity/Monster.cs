using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    [SerializeField] protected string _systemName;
    public string SystemName { get => _systemName; }
    public MonsterTable Data { get; protected set; }
    protected override void Awake()
    {
        base.Awake();

        Data = DataManager.GetDatas<MonsterTable>().Find((s)=>s._Name == _systemName);

        if(Data != null)
        {
            for(int i = 0; i < Data._Abilities.Length; ++i)
            {
                CharacterAbility ability = CharacterAbility.CharacterAbilityBuilderSet[Data._Abilities[i]]?.Invoke(this);
                if (ability != null)
                    AbilityStack.Add(ability);
            }
        }

        Status.InitStatus(Data);
    }

    protected void Start()
    {
        if (CurrentAction == null)
            EntityUtil.ChangeAction(this, Data._Name + "IdleAction");
    }
}
