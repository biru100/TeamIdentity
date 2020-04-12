using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIBase<SingletonClass> : MonoBehaviour where SingletonClass : UIBase<SingletonClass>
{
    protected static SingletonClass _instance;

    public static SingletonClass Instance
    {
        get
        {
            if (_instance == null)
            {
                SingletonClass instance = FindObjectOfType<SingletonClass>();
                if (instance == null)
                    instance = Instantiate(ResourceManager.GetResource<GameObject>("UI/" + typeof(SingletonClass).Name)
                        , FindObjectOfType<Canvas>().transform).GetComponent<SingletonClass>();

                _instance = instance;
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null || _instance == this)
        {
            _instance = (SingletonClass)this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        UIAnim = GetComponent<Animator>();
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public Animator UIAnim { get; protected set; }

    public virtual void StartInterface()
    {
        UIAnim?.Play("Start");
    }


    public virtual void StopInterface()
    {
        UIAnim?.Play("Stop");
    }
}
