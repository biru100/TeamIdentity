using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class CharacterAction
{
    public Character Owner { get; protected set; }

    public virtual void StartAction(Character owner)
    {
        Owner = owner;
    }

    public virtual void UpdateAction()
    {

    }

    public virtual void FinishAction()
    {

    }
}
