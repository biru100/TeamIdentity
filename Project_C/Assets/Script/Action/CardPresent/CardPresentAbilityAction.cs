using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CardPresentAbilityAction : CharacterAction
{

public static CardPresentAbilityAction GetInstance() { return ObjectPooling.PopObject<CardPresentAbilityAction>(); }

public override void StartAction(Character owner)
{
base.StartAction(owner);

        List<CardTable> list = DataManager.GetDatas<CardTable>().Where((ct)=>true).ToList();
        EffectiveUtility.SuffleList(ref list, 100);

        CardFindUIInterface.Instance.FindList = list.GetRange(0, 3);
        CardFindUIInterface.Instance.StartInterface();
        (Owner as NPC).IsUse = true;
}

public override void UpdateAction()
{
base.UpdateAction();
}

public override void FinishAction()
{
base.FinishAction();
}
}
