using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIBase : MonoBehaviour
{
    public Animator UIAnim { get; protected set; }

    protected virtual void Awake()
    {
        UIAnim = GetComponent<Animator>();
    }

    public virtual void StartInterface()
    {
        UIAnim.Play("Start");
    }


    public virtual void StopInterface()
    {
        UIAnim.Play("Stop");
    }
}
