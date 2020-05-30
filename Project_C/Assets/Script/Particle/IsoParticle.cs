using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoParticle : MonoBehaviour
{
    public static int _zFightingOffset = 0;
    public static int _zFightingPoolLenght = 5;

    public static IsoParticle CreateParticle(string particleName, Vector3 position, float zAngle, bool isAnimLifeTime = true, float lifeTime = 1f)
    {
        GameObject particle = ResourceManager.GetResource<GameObject>("Particles/" + particleName);

        if(particle == null || particle.GetComponent<IsoParticle>() == null)
        {
            return null;
        }

        IsoParticle iparticle = Instantiate(particle, position, Quaternion.identity).GetComponent<IsoParticle>();
        iparticle.Angle = zAngle;
        iparticle.LifeTime = lifeTime;
        iparticle.RenderChild.z_weight += _zFightingOffset * 0.021f;
        iparticle.RenderChild.TranslateIsometricToWorldCoordination();
        _zFightingOffset = (_zFightingOffset + 1) % _zFightingPoolLenght;
        return iparticle;
    }

    public RenderTransform RenderChild { get; set; }
    public Animator Anim { get; set; }
    public bool IsAnimationLifeTime { get; set; }

    public float Angle { get => _angle;
        set
        {
            RenderChild.Rotation = Quaternion.Euler(new Vector3(0f, 0f, value));
            _angle = value;
        }
    }
    public float LifeTime { get; set; }

    protected float _angle = 0f;
    protected float _currentLifeTime = 0f;

    public void RotationAnim(string animName, Quaternion rotation)
    {
        Anim.Play(animName + "_" + AnimUtil.GetRenderAngle(rotation));
    }

    protected virtual void Awake()
    {
        RenderChild = GetComponentInChildren<RenderTransform>();
        Anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        _currentLifeTime += Time.deltaTime;

        if(!IsAnimationLifeTime && _currentLifeTime >= LifeTime)
        {
            Destroy(gameObject);
        }

        if(IsAnimationLifeTime && Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.095f)
        {
            Destroy(gameObject);
        }
    }
}
