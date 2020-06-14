using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ManagedUIInterface<SingletonClass> : UIBase<SingletonClass> where SingletonClass : ManagedUIInterface<SingletonClass>
{
    public bool IsActive { get; protected set; }

    public virtual void StartInterface()
    {
        gameObject.SetActive(true);
        if (!IsActive)
            UIAnim.Play("start");
        IsActive = true;
    }

    public virtual void StopInterface()
    {
        StartCoroutine(HookAnimationFinished("stop", () => IsActive = false));
    }

    protected IEnumerator HookAnimationFinished(string animName, Action finishAction)
    {
        yield return null;
        UIAnim.Play(animName);

        //while (true)
        //{
        //    yield return null;

        //    if (UIAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
        //    {
        //        finishAction?.Invoke();
        //        break;
        //    }
        //}

        gameObject.SetActive(false);
    }
}
