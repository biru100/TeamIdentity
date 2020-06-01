using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TimeLineEvent
{
    public float CallTime { get; protected set; }
    public bool IsCalled { get; set; }

    public Action EventCall;

    public TimeLineEvent(float callTime, Action func)
    {
        CallTime = callTime;
        EventCall = func;
    }
}

[Serializable]
public abstract class CharacterAction : IPoolObject
{
    public Character Owner { get; protected set; }
    public float ElapsedTime { get; protected set; }
    public List<TimeLineEvent> TimelineEvents = new List<TimeLineEvent>();

    public virtual void StartAction(Character owner)
    {
        Owner = owner;
        ElapsedTime = 0f;
        TimelineEvents.Clear();
    }

    public virtual void UpdateAction()
    {
        ElapsedTime += Time.deltaTime;

        List<TimeLineEvent> timeEvents = TimelineEvents.FindAll((t) => !t.IsCalled && (t.CallTime < ElapsedTime));
        if (timeEvents != null)
        {
            foreach(var e in timeEvents)
            {
                e.IsCalled = true;
                e.EventCall?.Invoke();
            }
        }
    }

    public virtual void FinishAction()
    {
        ObjectPooling.PushObject(this);
    }

    public virtual void ClearMember()
    {
        ElapsedTime = 0f;
        TimelineEvents.Clear();
    }
}
