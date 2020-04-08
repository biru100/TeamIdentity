using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerDashAction : CharacterAction
{

public static PlayerDashAction Instance = new PlayerDashAction();

public override void StartAction(Character owner)
{
base.StartAction(owner);
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
